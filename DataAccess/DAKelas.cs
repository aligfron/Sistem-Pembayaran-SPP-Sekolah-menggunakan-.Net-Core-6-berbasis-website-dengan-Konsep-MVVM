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
    public class DAKelas
    {
        private readonly db_sppContext db;
        public DAKelas(db_sppContext _db)
        {
            db = _db;
        }
        public VMResponse<List<VMTbMKela>> GetByFilter(string filter)
        {
            VMResponse<List<VMTbMKela>> response = new VMResponse<List<VMTbMKela>>();
            try
            {
                response.Data = (
                    from a in db.TbMKelas
                    join b in db.TbMJurusans on a.JurusanId equals b.Id
                    where a.IsDeleted == false
                    && (a.NamaKelas!.Contains(filter))
                    select new VMTbMKela
                    {
                        Id = a.Id,
                        NamaKelas = a.NamaKelas,
                        JurusanId = a.JurusanId,
                        NamaJurusan = b.NamaJurusan,
                        CreatedBy = a.CreatedBy,
                        ModifiedBy = a.ModifiedBy,
                        IsDeleted = a.IsDeleted
                    }
                    ).ToList();
                response.Message = (response.Data.Count > 0)
                    ? $"{response.Data.Count} of Class(s) found successfully."
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
        public VMResponse<List<VMTbMKela>> GetByJurusanId(int? jurusanid)
        {
            VMResponse<List<VMTbMKela>> response = new VMResponse<List<VMTbMKela>>();
            try
            {
                response.Data = (
                    from a in db.TbMKelas
                    join b in db.TbMJurusans on a.JurusanId equals b.Id
                    where a.IsDeleted == false
                    && a.JurusanId == jurusanid
                    select new VMTbMKela
                    {
                        Id = a.Id,
                        NamaKelas = a.NamaKelas,
                        JurusanId = a.JurusanId,
                        NamaJurusan = b.NamaJurusan,
                        CreatedBy = a.CreatedBy,
                        ModifiedBy = a.ModifiedBy,
                        IsDeleted = a.IsDeleted
                    }
                    ).ToList();
                response.Message = (response.Data.Count > 0)
                    ? $"{response.Data.Count} of Class(s) found successfully."
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

        public VMResponse<VMTbMKela?> GetById(int id)
        {
            VMResponse<VMTbMKela?> response = new VMResponse<VMTbMKela?>();
            try
            {
                if (id > 0)
                {
                    response.Data = (

                        from a in db.TbMKelas
                        join b in db.TbMJurusans on a.JurusanId equals b.Id
                        where a.IsDeleted == false
                        && a.Id == id
                        select new VMTbMKela
                        {
                            Id = a.Id,
                            NamaKelas = a.NamaKelas,
                            JurusanId = a.JurusanId,
                            NamaJurusan = b.NamaJurusan,
                            CreatedBy = a.CreatedBy,
                            ModifiedBy = a.ModifiedBy,
                            IsDeleted = a.IsDeleted
                        }
                    ).FirstOrDefault();

                    if (response.Data != null)
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Message = $"{HttpStatusCode.OK} - Class Success Full";
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.Message = $"{HttpStatusCode.NoContent} - class does not exis";
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = $"{HttpStatusCode.BadRequest} - please input class";
                }
            }
            catch (Exception e)
            {

                response.Message = $"{HttpStatusCode.InternalServerError} - {e.Message}";
            }
            return response;
        }


        public VMResponse<VMTbMKela?> Create(VMTbMKela data)
        {
            var response = new VMResponse<VMTbMKela?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TbMKela newData = new TbMKela
                    {
                        NamaKelas = data.NamaKelas,
                        JurusanId = data.JurusanId,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false
                    };


                    db.Add(newData);
                    db.SaveChanges();
                    dbTrans.Commit();


                    response.Data = new VMTbMKela
                    {
                        NamaKelas = newData.NamaKelas,
                        JurusanId = newData.JurusanId,
                        CreatedBy = newData.CreatedBy,
                        CreatedOn = newData.CreatedOn
                    };
                    response.StatusCode = HttpStatusCode.Created;
                    response.Message = $"{HttpStatusCode.Created} - New Class successfully created.";
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


        public VMResponse<VMTbMKela?> Update(VMTbMKela data)
        {
            var response = new VMResponse<VMTbMKela?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {

                    var existingData = db.TbMKelas
                                         .FirstOrDefault(c => c.Id == data.Id && !c.IsDeleted);

                    if (existingData == null)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = $"{HttpStatusCode.NotFound} - Class Not Found";
                        return response;
                    }


                    existingData.NamaKelas = data.NamaKelas;
                    existingData.JurusanId = data.JurusanId;
                    existingData.ModifiedBy = data.ModifiedBy;
                    existingData.ModifiedOn = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();


                    response.Data = GetById(data.Id).Data;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = $"{HttpStatusCode.OK} - Class Has Been Updated";
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


        public VMResponse<VMTbMKela> Delete(int id, int userId)

        {
            VMResponse<VMTbMKela?> response = new VMResponse<VMTbMKela?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TbMKela? existingData = db.TbMKelas
                                                   .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
                    if (existingData == null)
                    {

                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = $"{HttpStatusCode.NotFound} - Class Not Fount";
                    }


                    existingData!.IsDeleted = true;
                    existingData.DeletedBy = userId;
                    existingData.DeletedOn = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();
                    dbTrans.Commit();

                    response.Data = new VMTbMKela
                    {
                        IsDeleted = existingData!.IsDeleted,
                        DeletedBy = existingData.DeletedBy,
                        DeletedOn = existingData.DeletedOn
                    };

                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = $"{HttpStatusCode.OK} - Class  Has been Deleted";

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
