import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AuthService } from 'src/app/services/auth.service';
import { Login } from '../../models';




@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  titulo = "Login";
  userName: string;
  password: string;
  

  constructor( private toastr: ToastrService 
    , private authService: AuthService
    , public router: Router) { }

  ngOnInit() {
    if(localStorage.getItem('token') !== null && this.authService.loggedIn()){
      this.router.navigate(['/dashboard']);
    }
    else{
      this.router.navigate(['/user/login']);
    }
  }

  login(){
    let model: Login;
    model = Object.assign({userName: this.userName, password: this.password});

    this.authService.login(model).subscribe(user => {
      
      this.router.navigate(['/dashboard']);
    },
    error => {
      this.toastr.error('Falha de login');
    });
  }

}
