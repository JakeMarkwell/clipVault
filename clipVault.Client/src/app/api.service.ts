// src/app/api.service.ts
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse } from '@angular/common/http'; //Import HttpResponse
import { Observable, throwError } from 'rxjs';
import { catchError, retry, map } from 'rxjs/operators';
import { environment } from '../environments/development';

@Injectable({
  providedIn: 'root', // Makes the service a singleton
})
export class ApiService {
  // Define the base URL here, use the environment.
  private api = environment.apiUrl;

  constructor(private http: HttpClient) {}

  getThumbnails(thumbnailId: string): Observable<Blob> {
    return this.http.get(`${this.api}/thumbnail/${thumbnailId}`, { responseType: 'blob' })
      .pipe(
        retry(1),
      );
  }
}
