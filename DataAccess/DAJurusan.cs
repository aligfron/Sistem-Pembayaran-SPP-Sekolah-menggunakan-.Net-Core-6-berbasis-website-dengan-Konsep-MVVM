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
    public class DAJurusan
    {
        private readonly db_sppContext db;
        public DAJurusan(db_sppContext _db) 
        { 
            db = _db;
        }
        public VMResponse<List<VMTbMJurusan>> GetByFilter(string filter)
        {
            VMResponse<List<VMTbMJurusan>> response = new VMResponse<List<VMTbMJurusan>>();
            try
            {
                response.Data = (
                    from a in db.TbMJurusans
                    where a.IsDeleted == false
                    && (a.NamaJurusan!.Contains(filter))
                    select new VMTbMJurusan
                    {
                        Id = a.Id,
                        NamaJurusan = a.NamaJurusan,
                        CreatedBy = a.CreatedBy,
                        ModifiedBy = a.ModifiedBy,
                        IsDeleted = a.IsDeleted
                    }
                    ).ToList();
                response.Message = (response.Data.Count > 0)
                    ? $"{response.Data.Count} of Major(s) found successfully."
                    : $"{HttpStatusCode.NoContent} - No data found";

                response.StatusCode = (response.Data.Count > 0)
                    ? HttpStatusCode.OK
                    : HttpStatusCode.NoContent;
            }
            catch (Exception ex)
            {
                response.Message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                response.StatusCode = HttpStatusCode.InternalServerError;
            }
            return response;
        }

        public VMResponse<VMTbMJurusan?> GetById(int id)
        {
            VMResponse<VMTbMJurusan?> response = new VMResponse<VMTbMJurusan?>();
            try
            {
                if (id > 0)
                {
                    response.Data = (

                        from c in db.TbMJurusans
                        where c.IsDeleted == false
                        && (c.Id == id)
                        select new VMTbMJurusan
                        {
                            Id = c.Id,
                            NamaJurusan = c.NamaJurusan,
                            CreatedBy = c.CreatedBy,
                            ModifiedBy = c.ModifiedBy,
                            IsDeleted = c.IsDeleted
                        }
                    ).FirstOrDefault();

                    if (response.Data != null)
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Message = $"{HttpStatusCode.OK} - Major Sukses Full";
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.Message = $"{HttpStatusCode.NoContent} - Major does not exis";
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = $"{HttpStatusCode.BadRequest} - please input Major";
                }
            }
            catch (Exception e)
            {

                response.Message = $"{HttpStatusCode.InternalServerError} - {e.Message}";
            }
            return response;
        }

        
        public VMResponse<VMTbMJurusan?> Create(VMTbMJurusan data)
        {
            var response = new VMResponse<VMTbMJurusan?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TbMJurusan newData = new TbMJurusan
                    {
                        NamaJurusan = data.NamaJurusan,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false
                    };


                    db.Add(newData);
                    db.SaveChanges();
                    dbTrans.Commit();


                    response.Data = new VMTbMJurusan{
                        NamaJurusan = newData.NamaJurusan,
                        CreatedBy = newData.CreatedBy,
                        CreatedOn = newData.CreatedOn
                    };
                    response.StatusCode = HttpStatusCode.Created;
                    response.Message = $"{HttpStatusCode.Created} - New Major successfully created.";
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


        public VMResponse<VMTbMJurusan?> Update(VMTbMJurusan data)
        {
            var response = new VMResponse<VMTbMJurusan?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {

                    var existingData = db.TbMJurusans
                                         .FirstOrDefault(c => c.Id == data.Id && !c.IsDeleted);

                    if (existingData == null)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = $"{HttpStatusCode.NotFound} - Major Not Found";
                        return response;
                    }


                    existingData.NamaJurusan = data.NamaJurusan;
                    existingData.ModifiedBy = data.ModifiedBy;
                    existingData.ModifiedOn = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();


                    response.Data = GetById(data.Id).Data;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = $"{HttpStatusCode.OK} - Major Has Been Updated";
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


        public VMResponse<VMTbMJurusan> Delete(int id, int userId)
        {
            VMResponse<VMTbMJurusan?> response = new VMResponse<VMTbMJurusan?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TbMJurusan? existingData = db.TbMJurusans
                                                   .FirstOrDefault(c => c.Id == id);
                    if (existingData == null)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = $"{HttpStatusCode.NotFound} - Major Not Found";
                        return response;
                    }

                    db.TbMJurusans.Remove(existingData); // Hard delete
                    db.SaveChanges();
                    dbTrans.Commit();

                    response.Data = new VMTbMJurusan
                    {
                        Id = existingData.Id,
                        NamaJurusan = existingData.NamaJurusan // Sesuaikan dengan properti yang ingin Anda tampilkan
                    };

                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = $"{HttpStatusCode.OK} - Major has been deleted permanently";
                }
                catch (Exception ex)
                {
                    dbTrans.Rollback();
                    response.Message = $"{HttpStatusCode.InternalServerError} - {ex.Message}";
                }
            }
            return response;
        }

    }
}
