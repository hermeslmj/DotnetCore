import { Component, OnInit } from '@angular/core';
import { eventoService } from 'src/app/services/evento.service';
import { BsModalService } from 'ngx-bootstrap/modal';
import { FormBuilder, FormGroup, Validators, FormArray, Form } from '@angular/forms';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { ToastrService } from 'ngx-toastr';
import { ptBrLocale } from 'ngx-bootstrap/locale';
import { defineLocale } from 'ngx-bootstrap/chronos';
import { Evento } from 'src/app/models';
import { ActivatedRoute } from '@angular/router';


defineLocale('pt-br', ptBrLocale);

@Component({
  selector: 'app-eventos-edit',
  templateUrl: './eventosEdit.component.html',
  styleUrls: ['./eventosEdit.component.css']
})
export class EventosEditComponent implements OnInit {

  titulo = "Editar Evento";
  registerForm: FormGroup;
  evento : Evento = new Evento();
  imagemURL = "assets/img/download.jpeg";
  dataEvento = "";
  fileNameToUpdate = "";
  file: File;
  


  constructor(
    private eventoService: eventoService
    , private toastr: ToastrService
    , private fb: FormBuilder
    , private localeService: BsLocaleService
    , private router: ActivatedRoute
  ) {
    this.localeService.use('pt-br');
  }

  get lotes(): FormArray {
    return <FormArray>this.registerForm.get('lotes');
  }

  get redesSociais(): FormArray {
    return <FormArray>this.registerForm.get('redesSociais');
  }

  ngOnInit() {
    this.validation();
    this.carregarEvento();
  
  }

  carregarEvento(){
    const idEvento = parseInt(this.router.snapshot.paramMap.get('id'));
    this.eventoService.getEventoById(idEvento).subscribe(
      (evento: Evento) => {
        this.evento = Object.assign({}, evento);
        this.fileNameToUpdate = evento.imagemUrl.toString();
        this.imagemURL = "http://localhost:5000/resources/img/"+this.evento.imagemUrl;
        this.evento.imagemUrl = "";
        this.registerForm.patchValue(this.evento);
        this.evento.lotes.forEach(lote => {
          this.lotes.push(this.criaLote(lote));
        });
        this.evento.redesSociais.forEach(redesocial => {
          this.redesSociais.push(this.criaRedeSocial(redesocial));
        });
      },
      error => {

      }
    );
  }

  SalvarEvento() {
    this.evento.id = this.registerForm.value.id;
    this.evento = Object.assign({}, this.registerForm.value);
    //this.evento.imagemUrl = this.fileNameToUpdate;

    this.uploadImagem();


    this.eventoService.putEvento(this.registerForm.value.id, this.evento).subscribe(
      (eventoEditado: Evento) => {
        console.log(eventoEditado);
        
        
        this.toastr.success('Editado com sucesso');
      },
      error => {
        this.toastr.error(error);
      }
    );
  }

  uploadImagem(){
    this.evento.imagemUrl = this.fileNameToUpdate;
    if(this.file)
      this.eventoService.postUpload(this.file).subscribe();
    this.imagemURL = "http://localhost:5000/resources/img/"+this.evento.imagemUrl;
  }

  validation() {
    this.registerForm = this.fb.group({
      id: [''],
      local: ['', Validators.required],
      dataEvento: ['', Validators.required],
      qtdPessoas: ['', [Validators.required, Validators.max(120000)]],
      imagemUrl: [''],
      telefone: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      tema: ['', [Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      lotes: this.fb.array([]) ,
      redesSociais: this.fb.array([])
    });
  }

  criaLote(lote: any): FormGroup {
    return this.fb.group({
      id: [lote.id],
      nome: [lote.nome, Validators.required],
      quantidade: [lote.quantidade, Validators.required],
      preco: [lote.preco, Validators.required],
      dataInicio: [lote.dataInicio],
      dataFim: [lote.dataFim]
    });
  }

  criaRedeSocial(redesocial: any): FormGroup  {
    return this.fb.group({
      id: [redesocial.id],
      nome: [redesocial.nome, Validators.required],
      url: [redesocial.url, Validators.required]
    });
  }

  onFileChange(file: FileList)
  {
    const reader = new FileReader();

    reader.onload = ( event:any ) => {
      this.imagemURL = event.target.result;
      this.file = event.target.files;
    }

    
    reader.readAsDataURL(file[0]);
  }

  adicionarRedeSocial(){
    this.redesSociais.push(this.criaRedeSocial({id: 0}));
  }

  adicionarLote() {
    this.lotes.push(this.criaLote({id: 0}));
  }

  removerRedeSocial(id: number) {
    this.redesSociais.removeAt(id);
  }

  removerLote(id: number){
    this.lotes.removeAt(id);
  }

}
