﻿using ApiPeliculas.Data;
using ApiPeliculas.Modelos;
using ApiPeliculas.Repositorio.IRepositorio;

namespace ApiPeliculas.Repositorio
{
    public class CategoriaRepositorio:ICategoriaRepositorio
    {
        public readonly ApplicationDbContext _bd;



        public CategoriaRepositorio(ApplicationDbContext bd) { 
            _bd = bd;
        }

        public bool ActualizarCategoria(Categoria categoria)
        {
           categoria.FechaCreacion = DateTime.UtcNow;
            var categoriaExistentes = _bd.Categorias.Find(categoria.Id);
            if (categoriaExistentes != null) { _bd.Entry(categoriaExistentes).CurrentValues.SetValues(categoria); }
            else {
                _bd.Categorias.Update(categoria);
            }
               
            return Guardar();
        }

        public bool BorrarCategoria(Categoria categoria)
        {
            _bd.Categorias.Remove(categoria);
            return Guardar();

        }

        public bool CrearCategoria(Categoria categoria)
        {
            categoria.FechaCreacion = DateTime.UtcNow;
            _bd.Categorias.Add(categoria);
            return Guardar();
        }

        public bool ExisteCategoria(int id)
        {

            return _bd.Categorias.Any(c => c.Id == id);
        }

        public bool ExisteCategoria(string nombre)
        {
            bool valor = _bd.Categorias.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        public Categoria GetCategoria(int CategoriaId)
        {


            return _bd.Categorias.FirstOrDefault(c => c.Id == CategoriaId);
           
        }

        public ICollection<Categoria> GetCategorias()
        {
            return _bd.Categorias.OrderBy(c => c.Nombre).ToList();
        }

        public bool Guardar()
        {
           return _bd.SaveChanges() >= 0?true:false;
        }
    }


}
