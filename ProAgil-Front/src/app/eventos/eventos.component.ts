import { Component, OnInit, TemplateRef } from '@angular/core';
import { eventoService } from '../services/evento.service';
import { Evento } from '../models';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  constructor(
    private eventoService: eventoService
    ,private modalService: BsModalService 
  ) { }

  _filtroLista: string = "";

  eventos: Evento[];
  eventoFiltrados: Evento[];

  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImg = false;

  modalRef: BsModalRef;

  get filtroLista(): string
  {
    return this._filtroLista
  }


  set filtroLista(v: string) {
    this._filtroLista = v;
    this.eventoFiltrados = (this._filtroLista) ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }

  openModal(template: TemplateRef<any>) {
    this.modalRef = this.modalService.show(template);
  }

  ngOnInit() {
    this.getEventos();
  }

  getEventos() { 
    this.eventoService.getEventos().subscribe(
      ( _eventos: Evento[] ) => { this.eventos = this.eventoFiltrados = _eventos },
      error => {
        console.error(error)
      }
    );
  }

  filtrarEventos(filtrarPor: any): Evento[]{
    filtrarPor = filtrarPor.toLocaleLowerCase();
    return this.eventos.filter(
      x => x.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1

    );
  }

  mostrarImagens(){
    this.mostrarImg = !this.mostrarImg;
  }

}
