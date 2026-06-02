import { Routes } from '@angular/router';
import { ReservationListComponent } from './components/reservation/reservation-list/reservation-list';
import { ReservationFormComponent } from './components/reservation/reservation-form/reservation-form';
import { ReservationDetailComponent } from './components/reservation/reservation-detail/reservation-detail';

export const routes: Routes = [
  { path: '', redirectTo: 'reservations', pathMatch: 'full' },
  { path: 'reservations', component: ReservationListComponent },
  { path: 'reservations/create', component: ReservationFormComponent },
  { path: 'reservations/edit/:id', component: ReservationFormComponent },
  { path: 'reservations/:id', component: ReservationDetailComponent },
];