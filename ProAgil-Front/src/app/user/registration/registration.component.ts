import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { User } from '../../models';
import { AuthService } from 'src/app/services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {

  registerForm: FormGroup;
  user: User;
  constructor(
      private fb: FormBuilder
    , private toastr: ToastrService 
    , private authService: AuthService
    , private router: Router
  ) { }

  ngOnInit() {
    this.validation()
  }
  
  validation(){
    this.registerForm = this.fb.group({
      "fullName": ["", Validators.required ],
      "email": ["", [Validators.required, Validators.email]],
      "userName": ["", Validators.required],
      "passwords": this.fb.group({
        "password": ["", [Validators.required, Validators.minLength(4)]],
        "confirmPassword": ["", Validators.required]
      },{ validator: this.compararSenhas })
    });
  } 

  cadastrarUsuario(){
    if(this.registerForm.valid){
      this.user = Object.assign({
        password: this.registerForm.get("passwords.password").value,
      }, this.registerForm.value);
      this.authService.register(this.user).subscribe(user => {
        this.router.navigate(['/user/login']);
        this.toastr.success("Cadastrado com sucesso");
      },
      error => {
        const err = error.error;
        error.array.forEach(element => {
          switch(element.code){
            case 'DuplicateUserName':
              this.toastr.error("Já existe cadstro com esse usuário");
              break;
            default:
              this.toastr.error(element.code);
              break;
          }
        });
      }
      
      );
    }
  }

  compararSenhas(fb: FormGroup){
    const frmCtrl = fb.get("confirmPassword");

    if(frmCtrl.errors == null ||  "mismatch" in frmCtrl.errors){
      if(fb.get('password').value !== frmCtrl.value){
        frmCtrl.setErrors({ mismatch: true });
      }
      else{
        frmCtrl.setErrors(null);
      }
    }

  }

}
