using DataModel;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using ViewModel;

namespace DataAccess
{
    public class DAProfilSekolah
    {
        private readonly db_sppContext db;
        public DAProfilSekolah(db_sppContext _db)
        {
            db = _db;
        }
        public VMResponse<VMTbSekolah?> GetAll()
        {
            VMResponse<VMTbSekolah?> response = new VMResponse<VMTbSekolah?>();
            try
            {
                response.Data = (
                        from c in db.TbSekolahs
                        where c.IsDelete == false
                        select new VMTbSekolah
                        {
                            Id = c.Id,
                            NamaSekolah = c.NamaSekolah,
                            Alamat = c.Alamat,
                            NoTlp = c.NoTlp,
                            BiayaSpp = c.BiayaSpp,
                            Email = c.Email,
                            Npsn = c.Npsn,
                            StatusSekolah = c.StatusSekolah,
                            CreatedBy = c.CreatedBy,
                            IsDelete = c.IsDelete
                        }
                    ).FirstOrDefault();

                    if (response.Data != null)
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Message = $"{HttpStatusCode.OK} - School Profile Sukses Full";
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.Message = $"{HttpStatusCode.NoContent} - School Profile does not exis";
                    }
                
            }
            catch (Exception e)
            {

                response.Message = $"{HttpStatusCode.InternalServerError} - {e.Message}";
            }
            return response;
        }
        public VMResponse<VMTbSekolah?> GetById(int id)
        {
            VMResponse<VMTbSekolah?> response = new VMResponse<VMTbSekolah?>();
            try
            {
                if (id > 0)
                {
                    response.Data = (

                        from c in db.TbSekolahs
                        where c.IsDelete == false
                        && (c.Id == id)
                        select new VMTbSekolah{
                            Id = c.Id,
                            NamaSekolah = c.NamaSekolah,
                            Alamat = c.Alamat,
                            NoTlp = c.NoTlp,
                            BiayaSpp = c.BiayaSpp,
                            Email = c.Email,
                            Npsn = c.Npsn,
                            StatusSekolah = c.StatusSekolah,
                            CreatedBy = c.CreatedBy,
                            IsDelete = c.IsDelete
                        }
                    ).FirstOrDefault();

                    if (response.Data != null)
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Message = $"{HttpStatusCode.OK} - School Profile Sukses Full";
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.Message = $"{HttpStatusCode.NoContent} - School Profile does not exis";
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = $"{HttpStatusCode.BadRequest} - please input School Profile";
                }
            }
            catch (Exception e)
            {

                response.Message = $"{HttpStatusCode.InternalServerError} - {e.Message}";
            }
            return response;
        }
        public VMResponse<VMTbSekolah?> Create(VMTbSekolah data)
        {
            var response = new VMResponse<VMTbSekolah?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TbSekolah newData = new TbSekolah
                    {

                        NamaSekolah = data.NamaSekolah,
                        Alamat = data.Alamat,
                        NoTlp = data.NoTlp,
                        BiayaSpp = data.BiayaSpp,
                        Email = data.Email,
                        Npsn = data.Npsn,
                        StatusSekolah = data.StatusSekolah,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = DateTime.Now,
                        IsDelete = false
                    };


                    db.Add(newData);
                    db.SaveChanges();
                    dbTrans.Commit();
                    
                    VMTbSekolah responseData = new VMTbSekolah
                    {
                        NamaSekolah = newData.NamaSekolah,
                        Alamat = newData.Alamat,
                        NoTlp = newData.NoTlp,
                        BiayaSpp = newData.BiayaSpp,
                        Email = newData.Email,
                        Npsn = newData.Npsn,
                        StatusSekolah = newData.StatusSekolah,
                        CreatedBy = newData.CreatedBy,
                        CreatedOn = newData.CreatedOn
                    };

                    // Mengisi response dengan data hasil konversi
                    response.Data = responseData;
                    response.StatusCode = HttpStatusCode.Created;
                    response.Message = $"{HttpStatusCode.Created} - New School Profile successfully created.";
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();


                    response.Data = null;
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                }
            }

            return response;
        }


        public VMResponse<VMTbSekolah?> Update(VMTbSekolah data)
        {
            var response = new VMResponse<VMTbSekolah?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {

                    var existingData = db.TbSekolahs
                                         .FirstOrDefault(c => c.Id == data.Id && !c.IsDelete);

                    if (existingData == null)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = $"{HttpStatusCode.NotFound} - School Profile Not Found";
                        return response;
                    }

                    existingData.NamaSekolah = data.NamaSekolah;
                    existingData.Alamat = data.Alamat;
                    existingData.NoTlp = data.NoTlp;
                    existingData.BiayaSpp = data.BiayaSpp;
                    existingData.Email = data.Email;
                    existingData.Npsn = data.Npsn;
                    existingData.StatusSekolah = data.StatusSekolah;
                    existingData.ModifiedBy = data.ModifiedBy;
                    existingData.ModifiedOn = DateTime.Now; 

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();


                    response.Data = GetById(data.Id).Data;

                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = $"{HttpStatusCode.OK} - School Profile Has Been Updated";
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    response.StatusCode = HttpStatusCode.InternalServerError;
                    response.Message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                }
            }
            return response;
        }
    }
}
