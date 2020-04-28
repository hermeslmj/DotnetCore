import { Lote, RedeSocial, Palestrante } from './' 


export class Evento {
    /**
     *
     */
    constructor() {}
    id: number;  
    local: string;
    dataEvento: Date;  
    tema: string; 
    qtdPessoas: number;
    imagemUrl: string;
    telefone: string;
    email: string ; 
    lotes: Lote[];
    redesSociais: RedeSocial[];
    palestrantesEventos: Palestrante[];
    
}
