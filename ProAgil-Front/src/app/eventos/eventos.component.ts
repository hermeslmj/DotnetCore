import { Component, OnInit, TemplateRef } from '@angular/core';
import { eventoService } from '../services/evento.service';
import { Evento } from '../models';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';

defineLocale('pt-br', ptBrLocale);
@Component({
  selector: 'app-eventos',
  templateUrl: './eventos.component.html',
  styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

  constructor(
    private eventoService: eventoService
    , private modalService: BsModalService 
    , private fb: FormBuilder
    , private localeService: BsLocaleService
    , private toastr: ToastrService 
  ) 
  { 
    this.localeService.use('pt-br');
  }

  titulo = "Eventos";

  _filtroLista: string = "";

  eventos: Evento[];
  eventoFiltrados: Evento[];
  evento: Evento;
  

  imagemLargura = 50;
  imagemMargem = 2;
  mostrarImg = false;
  bodyDeletarEvento = "";


  registerForm: FormGroup;
  file: File;

  get filtroLista(): string
  {
    return this._filtroLista
  }


  set filtroLista(v: string) {
    this._filtroLista = v;
    this.eventoFiltrados = (this._filtroLista) ? this.filtrarEventos(this.filtroLista) : this.eventos;
  }


  ngOnInit() {
    this.validation();
    this.getEventos();
  }


  openModal(template: any) {
    this.registerForm.reset();
    template.show();
  }

  openEditContatoModal(template: any, eventoId: number) {
    this.registerForm.reset();
    this.eventoService.getEventoById(eventoId).subscribe
    (
      (evento) => {
        this.registerForm.setValue({
          "id": evento.id,
          "tema": evento.tema,
          "local": evento.local,
          "dataEvento": evento.dataEvento,
          "qtdPessoas": evento.qtdPessoas,
          "imagemUrl": '',
          "email": evento.email,
          "telefone": evento.telefone
        });
        
        template.show();
      },
      error => {
        this.toastr.error(error);
      }
    

    )
  }

  salvarAlteracoes(template: any) {
      if(this.registerForm.valid)
      {
        this.evento = Object.assign({}, this.registerForm.value);
        if(this.registerForm.value.id == "" || this.registerForm.value.id == null)
        {
          this.evento.id = 0;
          this.uploadImagem();

          this.eventoService.postEvento(this.evento).subscribe(
            (novoEvento: Evento) => {
              console.log(novoEvento);
              template.hide();
              this.getEventos();
              this.toastr.success('Inserido com sucesso');
            },
            error => {
              this.toastr.error(error);
            }
          );
        }
        else
        {
          this.evento.id = this.registerForm.value.id;

          this.uploadImagem();


          this.eventoService.putEvento(this.registerForm.value.id, this.evento).subscribe(
            (eventoEditado: Evento) => {
              console.log(eventoEditado);
              template.hide();
              this.getEventos();
              this.toastr.success('Editado com sucesso');
            },
            error => {
              this.toastr.error(error);
            }
          );
        }
        
      }
  }

  excluirEvento(evento: Evento, template: any) {
    this.openModal(template);
    this.evento = evento;
    this.bodyDeletarEvento = `Tem certeza que deseja excluir o Evento: ${evento.tema}, Código: ${evento.tema}`;
  }
 
  confirmeDelete(template: any) {
    this.eventoService.deleteEvento(this.evento.id).subscribe(
      () => {
          template.hide();
          this.getEventos();
          this.toastr.success('Deletado com sucesso');
        }, error => {
          this.toastr.error(error);
        }
    );
  }

  validation() {
    this.registerForm = this.fb.group({
      id: [''],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemUrl: ['', Validators.required],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]]
    });
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

  onFileChange(event){
    const reader = new FileReader();
    if (event.target.files && event.target.files.length > 0)
    {
        this.file = event.target.files;
    }
  }

  uploadImagem(){
    this.eventoService.postUpload(this.file).subscribe();
    const nomeArquivo = this.evento.imagemUrl.split('\\', 3);
    this.evento.imagemUrl = nomeArquivo[2];
  }
}
