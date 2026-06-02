import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private baseUrl = 'http://localhost:5020/api';

  constructor(private http: HttpClient) {}

  // ── Customer ──────────────────────────────────────────
  getCustomers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/customer`);
  }

  // ── Restaurant ────────────────────────────────────────
  getRestaurants(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/restaurant`);
  }

  getActiveRestaurants(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/restaurant/active`);
  }

  // ── Table ─────────────────────────────────────────────
  getTables(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/table`);
  }

  getTablesByState(state: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/table/state/${state}`);
  }

  getTablesByRestaurant(restaurantId: number): Observable<any[]> {
  return this.http.get<any[]>(`${this.baseUrl}/table`).pipe(
    map((tables: any[]) => tables.filter(t => t.restaurantId === restaurantId && t.state === 1))
  );
}

  // ── MenuItem ──────────────────────────────────────────
  getMenuItems(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/menuitem`);
  }

  getAvailableMenuItems(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/menuitem/available`);
  }

  // ── Reservation ───────────────────────────────────────
  getReservations(): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/reservation`);
  }

  getReservationById(id: number): Observable<any> {
    return this.http.get<any>(`${this.baseUrl}/reservation/${id}`);
  }

  getReservationsByState(state: number): Observable<any[]> {
    return this.http.get<any[]>(`${this.baseUrl}/reservation/state/${state}`);
  }

  createReservation(reservation: any): Observable<any> {
    return this.http.post<any>(`${this.baseUrl}/reservation`, reservation);
  }

  updateReservation(id: number, reservation: any): Observable<any> {
    return this.http.put<any>(`${this.baseUrl}/reservation/${id}`, reservation);
  }

  updateReservationState(id: number, state: number): Observable<any> {
    return this.http.patch<any>(`${this.baseUrl}/reservation/${id}/state`, state);
  }

  deleteReservation(id: number): Observable<any> {
    return this.http.delete<any>(`${this.baseUrl}/reservation/${id}`);
  }
}