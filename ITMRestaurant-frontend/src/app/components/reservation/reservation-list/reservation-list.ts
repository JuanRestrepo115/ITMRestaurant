import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { ApiService } from '../../../services/api';

@Component({
  selector: 'app-reservation-list',
  imports: [CommonModule, RouterLink],
  templateUrl: './reservation-list.html',
  styleUrl: './reservation-list.scss'
})
export class ReservationListComponent implements OnInit {

  reservations: any[] = [];
  loading = true;
  error = '';

  stateLabels: { [key: number]: string } = {
    1: 'Pendiente',
    2: 'Confirmada',
    3: 'Cancelada',
    4: 'Completada'
  };

  stateColors: { [key: number]: string } = {
    1: 'pending',
    2: 'confirmed',
    3: 'cancelled',
    4: 'completed'
  };

  constructor(private apiService: ApiService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.loadReservations();
  }

loadReservations(): void {
  this.loading = true;
  this.apiService.getReservations().subscribe({
    next: (data) => {
      this.reservations = data;
      this.loading = false;
      this.cdr.detectChanges();
    },
    error: (err) => {
      this.error = 'Error al cargar las reservaciones';
      this.loading = false;
      console.log('Error:', err); 
    }
  });
}

  deleteReservation(id: number): void {
    if (confirm('¿Estás seguro de eliminar esta reservación?')) {
      this.apiService.deleteReservation(id).subscribe({
        next: () => {
          this.reservations = this.reservations.filter(r => r.id !== id);
        },
        error: (err) => {
          alert('Error al eliminar la reservación');
        }
      });
    }
  }
}