import { HttpClient } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { User } from '../../shared/types/user';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private http = inject(HttpClient);
  private baseUrl = environment.apiUrl;

  getUsers() {
    return this.http.get<User[]>(this.baseUrl + 'Users');
  }

  getUser(id: string) {
    return this.http.get<User>(this.baseUrl + 'Users/' + id);
  }

  getUserByUsername(username: string) {
    return this.http.get<User>(this.baseUrl + 'Users/username/' + username);
  }

  updateUser(user: User) {
    return this.http.put<User>(this.baseUrl + 'Users/', user);
  }

  deleteUser(id: string) {
    return this.http.delete(this.baseUrl + 'Users/' + id);
  }
}
