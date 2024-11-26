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
    
    public class DAUser
    {
        private db_sppContext db;
        private int jumlahattempt = 0;
        public DAUser(db_sppContext _db)
        {
            db = _db;
        }
        public VMResponse<VMTbMUser> GetByEmail(string email)
        {
            VMResponse<VMTbMUser?> response = new VMResponse<VMTbMUser?>();
            try
            {
                response.Data = (
                    from u in db.TbMUsers
                    join b in db.TbMBiodata on u.BiodataId equals b.Id
                    join r in db.TbMRoles on u.RoleId equals r.Id

                    join s in db.TbMSiswas on b.Id equals s.BiodataId
                    into HaveCurr
                    from s in HaveCurr.DefaultIfEmpty()
                    where u.IsDeleted == false && u.Email == email
                    select new VMTbMUser
                    {
                        Id = u.Id,
                        BiodataId = u.BiodataId,
                        RoleId = u.RoleId,
                        Email = u.Email,
                        Password = u.Password,
                        TahunMasuk = u.TahunMasuk,
                        LoginAttempt = u.LoginAttempt,
                        IsLocked = u.IsLocked,
                        LastLogin = u.LastLogin,
                        IsDeleted = u.IsDeleted,

                        Fullname = b.Fullname,
                        Images = b.Images,
                        RoleName = r.Name,

                        SiswaId = s.Id
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

        public VMResponse<VMTbMUser> Login(VMTbMUser data)
        {
            VMResponse<VMTbMUser> response = new VMResponse<VMTbMUser> ();
            using (IDbContextTransaction dbTrans = db.Database.BeginTransaction())
            {
                try
                {
                    VMTbMUser excistingData = GetByEmail(data.Email!).Data!;
                    if (excistingData == null)
                    {
                        response.Data = null;
                        response.StatusCode = HttpStatusCode.NotFound;
                        response.Message = $"{HttpStatusCode.NotFound} - User not found";
                        return response;
                    }
                    if(excistingData.IsLocked == true)
                    {
                        excistingData.LoginAttempt = 5;
                        response.StatusCode = HttpStatusCode.Locked;
                        response.Message = $"{HttpStatusCode.Forbidden} - Account is locked due to multiple failed login attempts.";
                        response.Data = excistingData;
                        return response;
                    }
                    if(excistingData.Password != data.Password)
                    {
                        excistingData.LoginAttempt++;
                        excistingData.IsLocked = false;
                        if(excistingData.LoginAttempt  >= 5)
                        {
                            excistingData.IsLocked = true;
                        }
                        TbMUser userLogin = new TbMUser()
                        {
                            Id = excistingData.Id,
                            BiodataId = excistingData.BiodataId,
                            RoleId = excistingData.RoleId,
                            Email = data.Email,
                            Password = excistingData.Password,
                            LoginAttempt = excistingData.LoginAttempt,
                            IsLocked = excistingData.IsLocked,
                            LastLogin = excistingData.LastLogin,
                            CreatedBy = excistingData.CreatedBy,
                            CreatedOn = DateTime.Now,
                            ModifiedBy = excistingData.ModifiedBy,
                            ModifiedOn = DateTime.Now,
                            DeletedBy = excistingData.DeletedBy,
                            DeletedOn = excistingData.DeletedOn,
                            IsDeleted = excistingData.IsDeleted
                        };
                        // Tandai entitas sebagai diubah
                        db.Update(userLogin);
                        db.SaveChanges(); // Simpan perubahan
                        dbTrans.Commit();
                        response.Data = GetByEmail(data.Email).Data;
                        response.StatusCode = HttpStatusCode.OK;
                        response.Message = $"{HttpStatusCode.OK}- successfully login";
                        return response;
                    }
                    response.Data = GetByEmail(data.Email).Data;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Message = $"{HttpStatusCode.Unauthorized}-Invalid Password";
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
