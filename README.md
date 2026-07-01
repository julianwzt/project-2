# Aplikasi Manajemen Data Mahasiswa

Aplikasi ini mengelola data mahasiswa dan jurusan dengan fitur ekspor Excel. Backend menggunakan Spring Boot dan frontend menggunakan VB.NET Windows Forms.

## Teknologi

- Backend: Java 21, Spring Boot 4.1.0, Spring Web MVC, Spring Data JPA
- Database: PostgreSQL
- Excel export: Apache POI
- Frontend: VB.NET, Windows Forms

## Struktur Proyek

- `backend/` - aplikasi backend
- `frontend/` - aplikasi desktop VB.NET

## Menjalankan Backend

1. Pastikan Java dan PostgreSQL sudah terinstall.
2. Ubah konfigurasi database di `backend/src/main/resources/application.properties`.
3. Jalankan backend:
   ```bash
   cd backend
   ./mvnw spring-boot:run
   ```

## Konfigurasi PostgreSQL

Contoh `application.properties`:

```properties
spring.datasource.url=jdbc:postgresql://localhost:5432/"NamaDatabase"
spring.datasource.username="NamaUser"
spring.datasource.password="PasswordUser"
spring.datasource.driver-class-name=org.postgresql.Driver
spring.jpa.hibernate.ddl-auto=update
spring.jpa.show-sql=true
spring.jpa.properties.hibernate.format_sql=true
```

## Struktur Database

Gunakan skrip SQL berikut untuk membuat tabel dan data contoh:

```sql
DROP TABLE IF EXISTS public.mahasiswa;
DROP TABLE IF EXISTS public.jurusan;

CREATE TABLE public.jurusan (
    id_jurusan SERIAL PRIMARY KEY,
    nama_jurusan VARCHAR(255) NOT NULL,
    fakultas VARCHAR(255) NOT NULL,
    jenjang VARCHAR(50) NOT NULL
);

CREATE TABLE public.mahasiswa (
    id SERIAL PRIMARY KEY,
    nama VARCHAR(255) NOT NULL,
    umur INT NOT NULL,
    nim VARCHAR(50) NOT NULL UNIQUE,
    tgl_lahir DATE NOT NULL,
    alamat TEXT,
    id_jurusan INT,
    CONSTRAINT fk_mahasiswa_jurusan
        FOREIGN KEY(id_jurusan)
        REFERENCES public.jurusan(id_jurusan)
        ON DELETE SET NULL
);

INSERT INTO public.jurusan (nama_jurusan, fakultas, jenjang) VALUES
('Teknik Informatika', 'Fakultas Informatika', 'S1'),
('Sistem Informasi', 'Fakultas Informatika', 'S1'),
('Teknik Elektro', 'Fakultas Teknik', 'S1'),
('Teknik Mesin', 'Fakultas Teknik', 'S1'),
('Desain Komunikasi Visual', 'Fakultas Industri Kreatif', 'D4'),
('Manajemen', 'Fakultas Ekonomi dan Bisnis', 'S1'),
('Akuntansi', 'Fakultas Ekonomi dan Bisnis', 'S1');

INSERT INTO public.mahasiswa (nama, umur, nim, tgl_lahir, alamat, id_jurusan) VALUES
('Budi Santoso', 21, '13519001', '2003-05-10', 'Jl. Merdeka No. 10', 1);
```

## Menjalankan Frontend

1. Buka `frontend/frontend.sln` di Visual Studio.
2. Build project.
3. Jalankan aplikasi.

> Frontend hanya mendukung Windows.

## Susunan Excel

Urutan kolom pada file Excel adalah:

1. Nama
2. NIM
3. Umur
4. Alamat
5. Jurusan
6. Fakultas
7. Jenjang

## Dokumentasi API

Buka jika backend berjalan:

```
http://localhost:8081/swagger-ui/index.html
```

## Catatan Singkat

- Backend default di port `8081`.
- `spring.jpa.hibernate.ddl-auto=update` akan memperbarui skema otomatis.
- Pastikan PostgreSQL aktif sebelum menjalankan backend.
