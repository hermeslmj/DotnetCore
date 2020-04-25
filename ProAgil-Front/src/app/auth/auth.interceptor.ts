import { Injectable } from '@angular/core';
import { HttpInterceptor, HttpEvent, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { tap } from 'rxjs/internal/operators/tap';

@Injectable()
export class AuthInterceptor implements HttpInterceptor {
    public constructor(private router: Router) {}
    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        if(localStorage.getItem('token') !== null){
            const reqClone = req.clone({
                headers: req.headers.set("Authorization","Bearer " + localStorage.getItem('token'))
            });

            return next.handle(reqClone).pipe(
                tap(
                    success => {

                    },
                    err => {
                        if(err.status == 401){
                            this.router.navigateByUrl('user/login');
                        }
                    }
                )
            );
          }
          else{
            return next.handle(req.clone());
          }
        
    }
}