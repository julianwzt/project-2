package com.technicaltest.backend.controller;

import java.util.List;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.technicaltest.backend.model.Jurusan;
import com.technicaltest.backend.repository.JurusanRepository;

@RestController
@RequestMapping("/api/jurusan")
public class JurusanController {

    @Autowired
    private JurusanRepository repo;

    @GetMapping
    public List<Jurusan> getAll() {
        return repo.findAll();
    }
}