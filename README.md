# Aplikasi Manajemen Data Mahasiswa

Aplikasi manajemen data mahasiswa modern dengan arsitektur **Microservices** yang siap di *deploy* ke lingkungan Kubernetes.

## Fitur Utama

* **CRUD Operasional**: Kelola data mahasiswa dengan relasi ke tabel Jurusan.
* **Search Real-time**: Fitur pencarian instan pada tabel untuk filter Nama/NIM.
* **Backend-Driven Export**: Fitur ekspor data ke format Excel (`.xlsx`) yang diproses langsung oleh server.
* **Cascade Dropdown**: Fakultas dan Jenjang otomatis menyesuaikan pilihan Jurusan.
* **Swagger Documentation**: Dokumentasi API interaktif untuk pengembang.
* **Kubernetes Ready**: Konfigurasi siap jalan di atas Minikube/Kubernetes.

## Teknologi

* **Backend**: Java 21, Spring Boot, Spring Data JPA, Apache POI (Excel generation).
* **Database**: PostgreSQL (Managed by Kubernetes).
* **Frontend**: VB.NET Windows Forms.
* **Deployment**: Kubernetes (Minikube).

## Cara Menjalankan

### 1. Menjalankan di Lingkungan Kubernetes (Direkomendasikan)

Untuk menjalankan aplikasi secara otomatis seperti standar industri:

1. Pastikan Minikube sudah terinstall dan berjalan: `minikube start`.
2. Arahkan Docker ke Minikube:
```bash
minikube -p minikube docker-env | Invoke-Expression

```


3. Build *image* backend di dalam Minikube:
```bash
cd backend
./mvnw clean package -DskipTests
docker build -t backend-api:1.0 .

```


4. Deploy ke klaster:
```bash
cd..
kubectl apply -f k8s/database.yaml
kubectl apply -f k8s/backend.yaml

```


5. Buka pintu akses agar bisa diakses aplikasi VB.NET:
```bash
kubectl port-forward svc/backend-service 8081:8081

```



### 2. Menjalankan Frontend

1. Buka `frontend/frontend.sln` di Visual Studio.
2. Build dan Jalankan proyek.
3. Aplikasi akan otomatis terhubung ke API backend melalui port `8081`.

## Dokumentasi API

Setelah backend berjalan, dokumentasi lengkap (Swagger) dapat diakses melalui:
`http://localhost:8081/swagger-ui.html`

## Struktur Database

Aplikasi ini sudah mendukung *auto-init*. Saat `database.yaml` dijalankan, skrip SQL berikut otomatis tereksekusi di dalam Pod PostgreSQL:

* `public.jurusan`: Menyimpan master data jurusan, fakultas, dan jenjang.
* `public.mahasiswa`: Menyimpan data mahasiswa dengan relasi *Foreign Key* ke tabel jurusan.

## Catatan untuk Developer

* Jika ada *Error* (seperti `UnsupportedClassVersionError`), pastikan versi Java di komputermu dan versi di `Dockerfile` sama-sama menggunakan **Java 21**.
* Gunakan fitur pencarian (kotak input di atas tabel) untuk memfilter data secara cepat.
* Fitur *Export* akan langsung mengunduh file `.xlsx` yang diracik oleh *backend* sesuai data terbaru di database.
