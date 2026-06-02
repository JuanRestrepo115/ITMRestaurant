import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { ApiService } from '../../../services/api';
import { ChangeDetectorRef } from '@angular/core';
@Component({
  selector: 'app-reservation-detail',
  imports: [CommonModule, RouterLink],
  templateUrl: './reservation-detail.html',
  styleUrl: './reservation-detail.scss'
})
export class ReservationDetailComponent implements OnInit {

  reservation: any = null;
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

  constructor(
    private apiService: ApiService,
    private route: ActivatedRoute,
    private cdr: ChangeDetectorRef
  ) {}

  ngOnInit(): void {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.loadReservation(+id);
    }
  }

loadReservation(id: number): void {
  this.apiService.getReservationById(id).subscribe({
    next: (data) => {
      this.reservation = data;
      this.loading = false;
      this.cdr.detectChanges();
    },
    error: (err) => {
      this.error = 'Error al cargar la reservación';
      this.loading = false;
      console.log('Error:', err); 
    }
  });
}

  getTotal(): number {
    if (!this.reservation?.reservationDetails) return 0;
    return this.reservation.reservationDetails.reduce(
      (acc: number, detail: any) => acc + (detail.quantity * detail.unitPrice), 0
    );
  }
}