import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Evento } from '../models';

@Injectable({
  providedIn: 'root'
})
export class eventoService {

  baseUrl = 'http://localhost:5000/api/Evento';


  constructor(private http: HttpClient) { }

  getEventos(): Observable<Evento[]>
  {
    return this.http.get<Evento[]>(this.baseUrl);
  }

  getEventoById(id: number): Observable<Evento>
  {
    return this.http.get<Evento>(`${this.baseUrl}/${id}`);
  }

  getEventoByTema(tema: string): Observable<Evento>
  {
    return this.http.get<Evento>(`${this.baseUrl}/getByTema/${tema}`);
  }

  postEvento(evento: Evento)
  {
    return this.http.post(this.baseUrl, evento);
  }

  putEvento(eventoId: number, evento: Evento) 
  {
    return this.http.put(`${this.baseUrl}/${eventoId}`, evento);
  }

  deleteEvento(eventoId: number) 
  {
    return this.http.delete(`${this.baseUrl}/${eventoId}`);
  }

  postUpload(file: File)
  {
    const fileToUpload = <File>file[0];
    const formData = new FormData();
    formData.append('file', fileToUpload, fileToUpload.name);
    
    return this.http.post(`${this.baseUrl}/upload`, formData);
  }

}
