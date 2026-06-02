import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router, ActivatedRoute, RouterLink } from '@angular/router';
import { ApiService } from '../../../services/api';
import { ChangeDetectorRef } from '@angular/core';
@Component({
  selector: 'app-reservation-form',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './reservation-form.html',
  styleUrl: './reservation-form.scss'
})
export class ReservationFormComponent implements OnInit {

  isEditMode = false;
  reservationId: number | null = null;
  loading = false;
  loadingData = true;
  error = '';
  success = '';

  // Datos para los dropdowns
  customers: any[] = [];
  restaurants: any[] = [];
  tables: any[] = [];
  allTables: any[] = [];
  menuItems: any[] = [];

  // Modelo del formulario
  reservation = {
    customerId: 0,
    restaurantId: 0,
    tableId: 0,
    reservationTime: '',
    numberOfGuests: 1,
    observations: '',
    reservationDetails: [] as any[]
  };

  // Plato seleccionado para agregar
  selectedMenuItem = 0;
  selectedQuantity = 1;

  constructor(
    private apiService: ApiService,
    private router: Router,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode = true;
      this.reservationId = +id;
    }
    this.loadFormData();
  }

  loadFormData(): void {
    // Cargar clientes
    this.apiService.getCustomers().subscribe({
      next: (data) => this.customers = data
    });

    // Cargar restaurantes activos
    this.apiService.getActiveRestaurants().subscribe({
      next: (data) => this.restaurants = data
    });

    // Cargar mesas disponibles
    this.apiService.getTablesByState(1).subscribe({
      next: (data) => {
        this.allTables = data; // 👈 Guardar todas
        this.tables = [];      // 👈 Iniciar vacío
      }
    });
    // Cargar menu items disponibles
    this.apiService.getAvailableMenuItems().subscribe({
      next: (data) => {
        this.menuItems = data;
        this.loadingData = false;
        this.cdr.detectChanges();
        // Si es modo edicion cargar la reservacion
        if (this.isEditMode && this.reservationId) {
          this.loadReservation(this.reservationId);
        }
      }
    });
    
  }

  loadReservation(id: number): void {
    this.apiService.getReservationById(id).subscribe({
      next: (data) => {
        this.reservation = {
          customerId: data.customerId,
          restaurantId: data.restaurantId,
          tableId: data.tableId,
          reservationTime: new Date(data.reservationTime).toISOString().slice(0, 16),
          numberOfGuests: data.numberOfGuests,
          observations: data.observations,
          reservationDetails: data.reservationDetails || []
        };
      },
      error: () => this.error = 'Error al cargar la reservación'
    });
  }

addMenuItem(): void {
  const selectedId = +this.selectedMenuItem;
  if (selectedId === 0) return;

  const menuItem = this.menuItems.find(m => m.id === selectedId);
  if (!menuItem) return;

  const exists = this.reservation.reservationDetails.find(d => d.menuItemId === selectedId);
  if (exists) {
    exists.quantity += this.selectedQuantity;
    return;
  }

  this.reservation.reservationDetails.push({
    menuItemId: menuItem.id,
    menuItemName: menuItem.name,
    quantity: this.selectedQuantity,
    unitPrice: menuItem.price
  });

  this.selectedMenuItem = 0;
  this.selectedQuantity = 1;
  this.cdr.detectChanges(); 
}

  removeMenuItem(menuItemId: number): void {
    this.reservation.reservationDetails = this.reservation.reservationDetails.filter(
      d => d.menuItemId !== menuItemId
    );
  }

  getTotal(): number {
    return this.reservation.reservationDetails.reduce(
      (acc, detail) => acc + (detail.quantity * detail.unitPrice), 0
    );
  }

  onSubmit(): void {
    this.loading = true;
    this.error = '';

    const payload = {
      customerId: this.reservation.customerId,
      restaurantId: this.reservation.restaurantId,
      tableId: this.reservation.tableId,
      reservationTime: new Date(this.reservation.reservationTime).toISOString(),
      numberOfGuests: this.reservation.numberOfGuests,
      observations: this.reservation.observations,
      reservationDetails: this.reservation.reservationDetails.map(d => ({
        menuItemId: d.menuItemId,
        quantity: d.quantity,
        unitPrice: d.unitPrice
      }))
    };

    if (this.isEditMode && this.reservationId) {
      this.apiService.updateReservation(this.reservationId, payload).subscribe({
        next: () => {
          this.success = 'Reservación actualizada correctamente';
          this.loading = false;
          setTimeout(() => this.router.navigate(['/reservations']), 1500);
        },
        error: (err) => {
          console.log('Error completo:', err);
          this.error = err.error?.message || 'Error al actualizar la reservación';
          this.loading = false;
        }
      });
    } else {
      this.apiService.createReservation(payload).subscribe({
        next: () => {
          this.success = 'Reservación creada correctamente';
          this.loading = false;
          setTimeout(() => this.router.navigate(['/reservations']), 1500);
        },
        error: (err) => {
          console.log('Error completo:', err);
          this.error = err.error?.message || 'Error al crear la reservación';
          this.loading = false;
        }
      });
    }
  }
  onRestaurantChange(): void {
  this.reservation.tableId = 0;
  this.tables = [];
  
  if (this.reservation.restaurantId === 0) return;

  const restaurantId = +this.reservation.restaurantId;
  this.tables = this.allTables.filter(
    t => t.restaurantId === restaurantId && t.state === 1
  );
  this.cdr.detectChanges();
}
}