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
    public class DASiswa
    {
        private readonly db_sppContext db;
        public DASiswa(db_sppContext _db)
        {
            db = _db;
        }
        public VMResponse<List<VMTbMSiswa>> GetByFilter(string filter)
        {
            VMResponse<List<VMTbMSiswa>> response = new VMResponse<List<VMTbMSiswa>>();
            try
            {
                response.Data = (
                    from a in db.TbMSiswas
                    join b in db.TbMBiodata on a.BiodataId equals b.Id                   
                    join c in db.TbMKelas on a.KelasId equals c.Id
                    join d in db.TbMJurusans on a.JurusanId equals d.Id
                    join e in db.TbMUsers on b.Id equals e.BiodataId
                    where a.IsDeleted == false
                    && (b.Fullname!.Contains(filter) || a.Nisn!.Contains(filter) || a.Nis!.Contains(filter) || c.NamaKelas!.Contains(filter) || d.NamaJurusan!.Contains(filter) || a.TahunMasuk!.Contains(filter))
                    select new VMTbMSiswa
                    {
                        Id = a.Id,
                        Fullname = b.Fullname,
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
                        CreatedBy = a.CreatedBy,
                        ModifiedBy = a.ModifiedBy,
                        IsDeleted = a.IsDeleted
                    }
                    ).ToList();
                response.Message = (response.Data.Count > 0)
                    ? $"{response.Data.Count} of Student(s) found successfully."
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
        public VMResponse<VMTbMSiswa> GetByNis(string nis)
        {
            VMResponse<VMTbMSiswa?> response = new VMResponse<VMTbMSiswa?>();
            try
            {
                if (nis != null)
                {
                    response.Data = (

                         from a in db.TbMSiswas
                         join b in db.TbMBiodata on a.BiodataId equals b.Id
                         join c in db.TbMKelas on a.KelasId equals c.Id
                         join d in db.TbMJurusans on a.JurusanId equals d.Id
                         join e in db.TbMUsers on b.Id equals e.BiodataId
                         where a.IsDeleted == false
                                 && a.Nis == nis
                         select new VMTbMSiswa
                         {
                             Id = a.Id,
                             Fullname = b.Fullname,
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
                             CreatedBy = a.CreatedBy,
                             ModifiedBy = a.ModifiedBy,
                             IsDeleted = a.IsDeleted
                         }
                    ).FirstOrDefault();

                    if (response.Data != null)
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Message = $"{HttpStatusCode.OK} - Student Success Full";
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.Message = $"{HttpStatusCode.NoContent} - Student does not exis";
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = $"{HttpStatusCode.BadRequest} - please input Student";
                }
            }
            catch (Exception e)
            {

                response.Message = $"{HttpStatusCode.InternalServerError} - {e.Message}";
            }
            return response;
        }
        public VMResponse<List<VMTbMSiswa>> GetByJurusandanKelas(int jurusanId, int kelasId, string? filter)
        {
            VMResponse<List<VMTbMSiswa>> response = new VMResponse<List<VMTbMSiswa>>();
            try
            {
                response.Data = (
                    from a in db.TbMSiswas
                    join b in db.TbMBiodata on a.BiodataId equals b.Id
                    join c in db.TbMKelas on a.KelasId equals c.Id
                    join d in db.TbMJurusans on a.JurusanId equals d.Id
                    join e in db.TbMUsers on b.Id equals e.BiodataId
                    where a.IsDeleted == false
                        && a.JurusanId == jurusanId
                        && a.KelasId == kelasId
                        && (string.IsNullOrEmpty(filter) ||
                            b.Fullname!.Contains(filter) ||
                            a.Nisn!.Contains(filter) ||
                            a.Nis!.Contains(filter) ||
                            c.NamaKelas!.Contains(filter) ||
                            d.NamaJurusan!.Contains(filter) ||
                            a.TahunMasuk!.Contains(filter))
                    select new VMTbMSiswa
                    {
                        Id = a.Id,
                        Fullname = b.Fullname,
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
                        CreatedBy = a.CreatedBy,
                        ModifiedBy = a.ModifiedBy,
                        IsDeleted = a.IsDeleted
                    }
                ).ToList();

                response.Message = (response.Data.Count > 0)
                    ? $"{response.Data.Count} of Student(s) found successfully."
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


        public VMResponse<VMTbMSiswa?> GetById(int id)
        {
            VMResponse<VMTbMSiswa?> response = new VMResponse<VMTbMSiswa?>();
            try
            {
                if (id > 0)
                {
                    response.Data = (

                         from a in db.TbMSiswas
                         join b in db.TbMBiodata on a.BiodataId equals b.Id
                         join c in db.TbMKelas on a.KelasId equals c.Id
                         join d in db.TbMJurusans on a.JurusanId equals d.Id
                         join e in db.TbMUsers on b.Id equals e.BiodataId
                         where a.IsDeleted == false
                                 && a.Id == id
                        select new VMTbMSiswa
                        {
                            Id = a.Id,
                            Fullname = b.Fullname,
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
                            CreatedBy = a.CreatedBy,
                            ModifiedBy = a.ModifiedBy,
                            IsDeleted = a.IsDeleted
                        }
                    ).FirstOrDefault();

                    if (response.Data != null)
                    {
                        response.StatusCode = HttpStatusCode.OK;
                        response.Message = $"{HttpStatusCode.OK} - Student Success Full";
                    }
                    else
                    {
                        response.StatusCode = HttpStatusCode.NoContent;
                        response.Message = $"{HttpStatusCode.NoContent} - Student does not exis";
                    }
                }
                else
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.Message = $"{HttpStatusCode.BadRequest} - please input Student";
                }
            }
            catch (Exception e)
            {

                response.Message = $"{HttpStatusCode.InternalServerError} - {e.Message}";
            }
            return response;
        }
        public VMResponse<VMTbMBiodatum> GetBiodata()
        {
            VMResponse<VMTbMBiodatum?> response = new VMResponse<VMTbMBiodatum?>();
            try
            {
                response.Data = (
                    from u in db.TbMBiodata
                    where u.IsDeleted == false
                    orderby u.Id descending
                    select new VMTbMBiodatum()
                    {
                        Id = u.Id,
                        Images = u.Images,
                        Fullname = u.Fullname,
                        MobilePhone = u.MobilePhone,
                        CreatedBy = u.CreatedBy,
                        CreatedOn = u.CreatedOn,
                        ModifiedBy = u.ModifiedBy,
                        ModifiedOn = u.ModifiedOn,
                        DeletedBy = u.DeletedBy,
                        DeletedOn = u.DeletedOn,
                        IsDeleted = false,
                    }
                    ).FirstOrDefault();
                response.StatusCode = (response.Data != null) ?
                        HttpStatusCode.OK :
                        HttpStatusCode.NotFound;
                response.Message = (response.Data != null) ?
                    $"{HttpStatusCode.OK} - User succesfully fetched!"
                    : $"{HttpStatusCode.NotFound} - User does not exist!";

            }
            catch (Exception ex)
            {
                response.Message = $"{HttpStatusCode.NoContent} - {ex.Message}";
            }
            return response!;
        }
        public VMResponse<VMTbMUser> GetByEmail(string Email)
        {
            VMResponse<VMTbMUser?> response = new VMResponse<VMTbMUser?>();
            try
            {
                response.Data = (
                    from u in db.TbMUsers
                    join b in db.TbMBiodata on u.BiodataId equals b.Id
                    join r in db.TbMRoles on u.RoleId equals r.Id
                    join c in db.TbMSiswas on b.Id equals c.BiodataId
                    where u.IsDeleted == false && u.Email == Email
                    select new VMTbMUser()
                    {
                        Id = u.Id,
                        BiodataId = u.BiodataId,
                        Images = b.Images,
                        Fullname = b.Fullname,
                        RoleName = r.Name,
                        RoleId = u.RoleId,
                        Email = u.Email,
                        Password = u.Password,
                        LoginAttempt = u.LoginAttempt,
                        IsLocked = u.IsLocked,
                        TahunMasuk = u.TahunMasuk,
                        LastLogin = u.LastLogin,
                        CreatedBy = u.CreatedBy,
                        CreatedOn = u.CreatedOn,
                        ModifiedBy = u.ModifiedBy,
                        ModifiedOn = u.ModifiedOn,
                        DeletedBy = u.DeletedBy,
                        DeletedOn = u.DeletedOn,
                        IsDeleted = false,
                        Nis = c.Nis,
                        Nisn = c.Nisn,
                        KelasId = c.KelasId,
                        JurusanId = c.JurusanId,
                    }
                    ).FirstOrDefault();
                response.StatusCode = (response.Data != null) ?
                        HttpStatusCode.OK :
                        HttpStatusCode.NotFound;
                response.Message = (response.Data != null) ?
                    $"{HttpStatusCode.OK} - User succesfully fetched!"
                    : $"{HttpStatusCode.NotFound} - User does not exist!";

            }
            catch (Exception ex)
            {
                response.Message = $"{HttpStatusCode.NoContent} - {ex.Message}";
            }
            return response!;
        }

        public VMResponse<VMTbMUser> GetByIduser(int Id)
        {
            VMResponse<VMTbMUser?> response = new VMResponse<VMTbMUser?>();
            try
            {
                response.Data = (
                    from u in db.TbMUsers
                    join b in db.TbMBiodata on u.BiodataId equals b.Id
                    join r in db.TbMRoles on u.RoleId equals r.Id
                    join c in db.TbMSiswas on b.Id equals c.BiodataId
                    where u.IsDeleted == false && u.Id == Id
                    select new VMTbMUser()
                    {
                        Id = u.Id,
                        BiodataId = u.BiodataId,
                        Images = b.Images,
                        Fullname = b.Fullname,
                        RoleName = r.Name,
                        RoleId = u.RoleId,
                        Email = u.Email,
                        Password = u.Password,
                        LoginAttempt = u.LoginAttempt,
                        IsLocked = u.IsLocked,
                        TahunMasuk = u.TahunMasuk,
                        LastLogin = u.LastLogin,
                        CreatedBy = u.CreatedBy,
                        CreatedOn = u.CreatedOn,
                        ModifiedBy = u.ModifiedBy,
                        ModifiedOn = u.ModifiedOn,
                        DeletedBy = u.DeletedBy,
                        DeletedOn = u.DeletedOn,
                        IsDeleted = false,
                        Nis = c.Nis,
                        Nisn = c.Nisn,
                        KelasId = c.KelasId,
                        JurusanId = c.JurusanId,
                    }
                    ).FirstOrDefault();
                response.StatusCode = (response.Data != null) ?
                        HttpStatusCode.OK :
                        HttpStatusCode.NotFound;
                response.Message = (response.Data != null) ?
                    $"{HttpStatusCode.OK} - User succesfully fetched!"
                    : $"{HttpStatusCode.NotFound} - User does not exist!";

            }
            catch (Exception ex)
            {
                response.Message = $"{HttpStatusCode.NoContent} - {ex.Message}";
            }
            return response!;
        }
        public VMResponse<VMTbMUser?> Create(VMTbMUser data)
        {
            var response = new VMResponse<VMTbMUser?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    // Step 1: Insert into TbMBiodatum
                    TbMBiodatum biodata = new TbMBiodatum()
                    {
                        Fullname = data.Fullname,
                        MobilePhone = data.MobilePhone,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = DateTime.Now,
                    };
                    db.Add(biodata);
                    db.SaveChanges();

                    // Step 2: Retrieve user bio information
                    VMTbMBiodatum? userBio = GetBiodata().Data;
                    if (userBio == null)
                    {
                        throw new Exception("User biodata could not be retrieved.");
                    }

                    // Step 3: Insert into TbMUser with bio data
                    TbMUser user = new TbMUser()
                    {
                        BiodataId = biodata.Id,
                        Email = data.Email,
                        Password = data.Password,
                        RoleId = data.RoleId,
                        LoginAttempt = 0,
                        TahunMasuk = data.TahunMasuk,
                        IsLocked = false,
                        LastLogin = DateTime.Now,
                        CreatedBy = data.CreatedBy,
                        CreatedOn = DateTime.Now,
                        IsDeleted = false
                    };
                    db.Update(user);
                    db.SaveChanges();

                    // Step 4: If RoleId is 2, insert into TbMSiswa
                    if (data.RoleId == 2)
                    {
                        TbMSiswa admin = new TbMSiswa()
                        {
                            Nis = data.Nis,
                            Nisn = data.Nisn,
                            KelasId = data.KelasId,
                            JurusanId = data.JurusanId,
                            BiodataId = biodata.Id,
                            TahunMasuk = data.TahunMasuk,
                            CreatedBy = data.CreatedBy,
                            CreatedOn = DateTime.Now,
                        };
                        db.Add(admin);
                        db.SaveChanges();
                    }

                    // Commit transaction if all operations succeed
                    dbTrans.Commit();

                    // Populate response with new user data
                    response.Data = GetByEmail(data.Email)?.Data;
                    response.StatusCode = HttpStatusCode.Created;
                    response.Message = $"{HttpStatusCode.Created} - New Student successfully created.";
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

        public VMResponse<VMTbMUser?> Update(VMTbMUser data)
        {
            var response = new VMResponse<VMTbMUser?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    // Step 1: Update TbMBiodata
                    var existingData = db.TbMBiodata
                                         .FirstOrDefault(c => c.Id == data.BiodataId && !c.IsDeleted);
                    if (existingData == null)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = $"{HttpStatusCode.NotFound} - Biodata Not Found";
                        return response;
                    }

                    existingData.Fullname = data.Fullname;
                    existingData.MobilePhone = data.MobilePhone;
                    existingData.ModifiedBy = data.ModifiedBy;
                    existingData.ModifiedOn = DateTime.Now;
                    db.Update(existingData);
                    db.SaveChanges();

                    // Step 2: Update TbMUser
                    var existingDataUser = db.TbMUsers
                                          .FirstOrDefault(c => c.BiodataId == data.BiodataId && !c.IsDeleted);

                    if (existingDataUser == null)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = $"{HttpStatusCode.NotFound} - User Not Found";
                        return response;
                    }

                    existingDataUser.Email = data.Email;
                    existingDataUser.TahunMasuk = data.TahunMasuk;
                    existingDataUser.ModifiedBy = data.ModifiedBy;
                    existingDataUser.ModifiedOn = DateTime.Now;

                    db.Update(existingDataUser);
                    db.SaveChanges();

                    // Step 3: Update TbMSiswa
                    var existingDataSiswa = db.TbMSiswas
                                              .FirstOrDefault(c => c.BiodataId == data.BiodataId && !c.IsDeleted);
                    if (existingDataSiswa == null)
                    {
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = $"{HttpStatusCode.NotFound} - Siswa Not Found";
                        return response;
                    }

                    existingDataSiswa.Nis = data.Nis;
                    existingDataSiswa.Nisn = data.Nisn;
                    existingDataSiswa.KelasId = data.KelasId;
                    existingDataSiswa.JurusanId = data.JurusanId;
                    existingDataSiswa.TahunMasuk = data.TahunMasuk;
                    existingDataSiswa.ModifiedBy = data.ModifiedBy;
                    existingDataSiswa.ModifiedOn = DateTime.Now;

                    db.Update(existingDataSiswa);
                    db.SaveChanges();

                    // Commit once after all operations
                    dbTrans.Commit();

                    // Set response
                    response.Data = new VMTbMUser
                    {
                        Fullname = existingData.Fullname,
                        MobilePhone = existingData.MobilePhone,
                        ModifiedBy = existingData.ModifiedBy,
                        Email = existingDataUser.Email,
                        TahunMasuk = existingDataUser.TahunMasuk,
                        Nis = existingDataSiswa.Nis,
                        Nisn = existingDataSiswa.Nisn,
                        KelasId = existingDataSiswa.KelasId,
                        JurusanId = existingDataSiswa.JurusanId
                    };
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = $"{HttpStatusCode.OK} - User successfully updated.";
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



        public VMResponse<VMTbMBiodatum> Delete(int id, int userId)

        {
            VMResponse<VMTbMBiodatum?> response = new VMResponse<VMTbMBiodatum?>();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    TbMBiodatum? existingData = db.TbMBiodata
                                                   .FirstOrDefault(c => c.Id == id && !c.IsDeleted);
                    if (existingData == null)
                    {

                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = $"{HttpStatusCode.NotFound} - Student Not Fount";
                    }


                    existingData!.IsDeleted = true;
                    existingData.DeletedBy = userId;
                    existingData.DeletedOn = DateTime.Now;

                    db.Update(existingData);
                    db.SaveChanges();

                    var existingDataUser = db.TbMUsers
                                          .FirstOrDefault(c => c.BiodataId == id && !c.IsDeleted);
                    existingDataUser!.IsDeleted = true;
                    existingDataUser.DeletedBy = userId;
                    existingDataUser.DeletedOn = DateTime.Now;

                    db.Update(existingDataUser);
                    db.SaveChanges();
                    var existingDataSiswa = db.TbMSiswas
                                              .FirstOrDefault(c => c.BiodataId == id && !c.IsDeleted);
                    existingDataSiswa!.IsDeleted = true;
                    existingDataSiswa.DeletedBy = userId;
                    existingDataSiswa.DeletedOn = DateTime.Now;

                    db.Update(existingDataSiswa);
                    db.SaveChanges();
                    dbTrans.Commit();

                    response.Data = new VMTbMBiodatum
                    {
                        IsDeleted = existingData!.IsDeleted,
                        DeletedBy = existingData.DeletedBy,
                        DeletedOn = existingData.DeletedOn
                    };

                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = $"{HttpStatusCode.OK} - Student  Has been Deleted";

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
