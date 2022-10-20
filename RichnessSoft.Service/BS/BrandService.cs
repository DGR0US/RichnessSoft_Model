using Microsoft.EntityFrameworkCore;
using RichnessSoft.Entity.Context;
using RichnessSoft.Entity.Model;
using RichnessSoft.Service.Store;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RichnessSoft.Service.BS
{
    public interface IBrandService
    {
        ResultModel GetAll(int CorpId);
        Task<ResultModel> GetAllAsync(int CorpId);

        ResultModel GetById(int Id);
        ResultModel GetByCode(int CorpId, string Code);
        ResultModel GetByName(int CorpId, string Name);
        ResultModel Add(Brand brand);
        ResultModel Edit(Brand brand);
        ResultModel Delete(Brand brand);
    }
    public class BrandService : BaseService , IBrandService
    {
        private readonly RicnessDbContext _db;
        private readonly ProfileStore _store;
        public BrandService(RicnessDbContext db, ProfileStore store)
        {
            _db = db;
            _store = store;
        }

        public ResultModel Add(Brand brand)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    brand.createby = _store.CurrentUser.username;
                    brand.createatutc = DateTime.Now;
                    brand.companyid = _store.CurentCompany.id;
                    db.Add(brand);
                    db.SaveChanges();
                    AddLog<Brand>(brand);
                    res.Success = true;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message.ToString();
            }
            return res;
        }

        public ResultModel Delete(Brand brand)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    var data = db.Brand.Where(x => x.id == brand.id).FirstOrDefault();
                    db.Brand.Remove(data);
                    DeleteLog<Brand>(data);
                    db.SaveChanges();
                    res.Success = true;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message.ToString();
            }
            return res;
        }

        public ResultModel Edit(Brand brand)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    var Olddata = db.Brand.Where(x => x.id == brand.id).FirstOrDefault();
                    brand.updateby = _store.CurrentUser.username;
                    brand.companyid = _store.CurentCompany.id;
                    brand.updateatutc = DateTime.Now;
                    db.Brand.Update(brand);
                    db.SaveChanges();
                    _db.Entry(brand).State = EntityState.Detached;
                    UpdateLog<Brand>(Olddata, brand);
                    res.Success = true;
                }
            }
            catch (Exception ex)
            {
                res.Success = false;
                res.Message = ex.Message.ToString();
            }
            return res;
        }

        public ResultModel GetAll(int CorpId)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Brand.Where(x => x.companyid == CorpId).ToList();
            return res;
        }

        public async Task<ResultModel> GetAllAsync(int CorpId)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Brand.Where(x => x.companyid == CorpId).ToList();
            return res;
        }

        public ResultModel GetByCode(int CorpId, string Code)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Brand.Where(x => x.companyid == CorpId && x.code.Equals(Code)).FirstOrDefault();
            return res;
        }

        public ResultModel GetById(int Id)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Brand.Where(x => x.id == Id).FirstOrDefault();
            return res;
        }

        public ResultModel GetByName(int CorpId, string Name)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Brand.Where(x => x.companyid == CorpId && x.code.Contains(Name)).ToList();
            return res;
        }
    }
}
