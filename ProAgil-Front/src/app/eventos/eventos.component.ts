import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  constructor(private http: HttpClient) { }

  _filtroLista: string = "";

  get filtroLista(): string
  {
    return this._filtroLista
  }

  set filtroLista(v: string) {
    this._filtroLista = v;
    this.eventoFiltrados = (this._filtroLista) ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }



  eventos: any = [];
  eventoFiltrados: any = [];

  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImg = false;
  

  ngOnInit() {
    this.getEventos();
  }

  getEventos() { 
    this.http.get("http://localhost:5000/Evento").subscribe(
      response => { this.eventos= this.eventoFiltrados = response },
      error => {
        console.error(error)
      }
    );
  }

  filtrarEventos(filtrarPor: any){
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      x => x.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
    );
  }

  mostrarImagens(){
    this.mostrarImg = !this.mostrarImg;
  }

}
