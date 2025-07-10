# API Diskografi Band XH
API Diskografi Band XH adalah proyek Web API sederhana berbasis ASP.NET Core untuk mengelola data lagu dan member xdinary heroes. API ini terdapat fitur CRUD dan autentikasi user untuk mengakses fitur CRUD tersebut menggunakan JWT. Dibuat sebagai bagian dari tugas mata kuliah, proyek ini belum dideploy dan hanya berjalan secara lokal.

## Endpoint
### Autentikasi
- `POST` /register
- `POST` /login

### CRUD Member
- `GET` /api/member
- `POST` /api/member/add
- `PUT` /api/member/update/{Id}
- `DELETE` /api/member/delete/{Id}
  
### CRUD Lagu
- `GET` /api/song
- `POST` /api/song/add
- `PUT` /api/song/update/{Id}
- `DELETE` /api/song/delete/{Id}
