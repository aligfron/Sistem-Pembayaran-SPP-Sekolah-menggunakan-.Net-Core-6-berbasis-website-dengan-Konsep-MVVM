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
    public class DAPembayaran
    {
        private readonly db_sppContext db;
        public DAPembayaran(db_sppContext _db)
        {
            db = _db;
        }
        public VMResponse<List<VMTbTPembayaran>> GetByIdSiswa(int id)
        {
            VMResponse<List<VMTbTPembayaran>> response = new VMResponse<List<VMTbTPembayaran>>();
            try
            {
                if (id > 0)
                {
                    response.Data = (
                         from f in db.TbTPembayarans
                         join a in db.TbMSiswas on f.SiswaId equals a.Id
                         join b in db.TbMBiodata on a.BiodataId equals b.Id
                         join c in db.TbMKelas on a.KelasId equals c.Id
                         join d in db.TbMJurusans on a.JurusanId equals d.Id
                         join e in db.TbMUsers on b.Id equals e.BiodataId
                         where f.IsDeleted == false
                                 && f.SiswaId == id
                         orderby f.CreatedOn descending // Mengurutkan berdasarkan tanggal terbaru
                         select new VMTbTPembayaran
                         {
                             Id = f.Id,
                             Fullname = b.Fullname,
                             SiswaId = f.SiswaId,
                             Bulan = f.Bulan,
                             Tahun = f.Tahun,
                             Jumlah = f.Jumlah,
                             Nisn = a.Nisn,
                             Nis = a.Nis,
                             KelasId = a.KelasId,
                             BiodataId = a.BiodataId,
                             NamaKelas = c.NamaKelas,
                             JurusanId = a.JurusanId,
                             NamaJurusan = d.NamaJurusan,
                             TahunMasuk = a.TahunMasuk,
                             Email = e.Email,
                             MobilePhone = b.MobilePhone,
                             Images = b.Images,
                             CreatedBy = f.CreatedBy,
                             CreatedOn = f.CreatedOn,
                             ModifiedBy = f.ModifiedBy,
                             IsDeleted = f.IsDeleted
                         }
                    ).Take(3).ToList(); // Mengambil maksimal 3 data terbaru

                    if (response.Data.Count > 0)
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Message = $"{HttpStatusCode.OK} - Payment History Retrieved Successfully";
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.Message = $"{HttpStatusCode.NoContent} - Payment History Does Not Exist";
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = $"{HttpStatusCode.BadRequest} - Please Input Valid Payment History ID";
                }
            }
            catch (Exception e)
            {
                response.Message = $"{HttpStatusCode.InternalServerError} - {e.Message}";
            }
            return response;
        }

        public VMResponse<VMTbTPembayaran?> Create(VMTbTPembayaran data)
        {
            var response = new VMResponse<VMTbTPembayaran?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TbTPembayaran newData = new TbTPembayaran
                    {
                        SiswaId = data.SiswaId,
                        Bulan = data.Bulan,
                        Tahun = data.Tahun,
                        Jumlah = data.Jumlah,
                        KelasId = data.KelasId,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false
                    };


                    db.Add(newData);
                    db.SaveChanges();
                    dbTrans.Commit();


                    response.Data = new VMTbTPembayaran
                    {
                        SiswaId = newData.SiswaId,
                        Bulan = newData.Bulan,
                        Tahun = newData.Tahun,
                        Jumlah = newData.Jumlah,
                        KelasId = newData.KelasId,
                        CreatedBy = newData.CreatedBy,
                        CreatedOn = newData.CreatedOn
                    };
                    response.StatusCode = HttpStatusCode.Created;
                    response.Message = $"{HttpStatusCode.Created} - New Payment successfully created.";
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
        public VMResponse<VMTbTPembayaran?> GetById(int id)
        {
            VMResponse<VMTbTPembayaran?> response = new VMResponse<VMTbTPembayaran?>();
            try
            {
                if (id > 0)
                {
                    response.Data = (
                        from f in db.TbTPembayarans
                        join a in db.TbMSiswas on f.SiswaId equals a.Id
                        join b in db.TbMBiodata on a.BiodataId equals b.Id
                        join c in db.TbMKelas on a.KelasId equals c.Id
                        join d in db.TbMJurusans on a.JurusanId equals d.Id
                        join e in db.TbMUsers on b.Id equals e.BiodataId
                        where f.IsDeleted == false
                                && f.Id == id
                        select new VMTbTPembayaran
                        {
                            Id = f.Id,
                            Fullname = b.Fullname,
                            SiswaId = f.SiswaId,
                            Bulan = f.Bulan,
                            Tahun = f.Tahun,
                            Jumlah = f.Jumlah,
                            Nisn = a.Nisn,
                            Nis = a.Nis,
                            KelasId = a.KelasId,
                            BiodataId = a.BiodataId,
                            NamaKelas = c.NamaKelas,
                            JurusanId = a.JurusanId,
                            NamaJurusan = d.NamaJurusan,
                            TahunMasuk = a.TahunMasuk,
                            Email = e.Email,
                            MobilePhone = b.MobilePhone,
                            Images = b.Images,
                            CreatedBy = f.CreatedBy,
                            CreatedOn = f.CreatedOn,
                            ModifiedBy = f.ModifiedBy,
                            IsDeleted = f.IsDeleted
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

    }
}
