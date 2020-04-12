import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-barra-titulo',
  templateUrl: './barra-titulo.component.html',
  styleUrls: ['./barra-titulo.component.css']
})
export class BarraTituloComponent implements OnInit {

  @Input() title: string;

  constructor() { }

  ngOnInit() {
  }

}
