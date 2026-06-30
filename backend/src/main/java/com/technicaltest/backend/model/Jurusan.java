package com.technicaltest.backend.model;

import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.GenerationType;
import jakarta.persistence.Id;
import jakarta.persistence.Table;

@Entity
@Table(name = "jurusan")
public class Jurusan {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    private Long idJurusan;

    private String namaJurusan;
    private String fakultas;
    private String jenjang;
    public Long getIdJurusan() {
        return idJurusan;
    }
    public void setIdJurusan(Long idJurusan) {
        this.idJurusan = idJurusan;
    }
    public String getNamaJurusan() {
        return namaJurusan;
    }
    public void setNamaJurusan(String namaJurusan) {
        this.namaJurusan = namaJurusan;
    }
    public String getFakultas() {
        return fakultas;
    }
    public void setFakultas(String fakultas) {
        this.fakultas = fakultas;
    }
    public String getJenjang() {
        return jenjang;
    }
    public void setJenjang(String jenjang) {
        this.jenjang = jenjang;
    }

    
}