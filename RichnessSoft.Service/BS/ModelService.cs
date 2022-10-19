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
    public interface IModelService
    {
        ResultModel GetAll(int CorpId);
        Task<ResultModel> GetAllAsync(int CorpId);

        ResultModel GetById(int Id);
        ResultModel GetByCode(int CorpId, string Code);
        ResultModel GetByName(int CorpId, string Name);
        ResultModel Add(Models md);
        ResultModel Edit(Models md);
        ResultModel Delete(Models md);
    }
    public class ModelService : BaseService, IModelService
    {
        private readonly RicnessDbContext _db;
        private readonly ProfileStore _store;
        public ModelService(RicnessDbContext db, ProfileStore store)
        {
            _db = db;
            _store = store;
        }

        public ResultModel Add(Models md)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    md.createby = _store.CurrentUser.username;
                    md.createatutc = DateTime.Now;
                    md.companyid = _store.CurentCompany.id;
                    db.Add(md);
                    db.SaveChanges();
                    AddLog<Models>(md);
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

        public ResultModel Delete(Models md)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    var data = db.Models.Where(x => x.id == md.id).FirstOrDefault();
                    db.Models.Remove(data);
                    DeleteLog<Models>(data);
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

        public ResultModel Edit(Models md)
        {
            ResultModel res = new ResultModel();
            try
            {
                using (var db = new RicnessDbContext())
                {
                    var Olddata = db.Models.Where(x => x.id == md.id).FirstOrDefault();
                    md.updateby = _store.CurrentUser.username;
                    md.companyid = _store.CurentCompany.id;
                    md.updateatutc = DateTime.Now;
                    db.Models.Update(md);
                    db.SaveChanges();
                    _db.Entry(md).State = EntityState.Detached;
                    UpdateLog<Models>(Olddata, md);
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
            res.Data = _db.Models.Where(x => x.companyid == CorpId).ToList();
            return res;
        }

        public async Task<ResultModel> GetAllAsync(int CorpId)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Models.Where(x => x.companyid == CorpId).ToList();
            return res;
        }

        public ResultModel GetByCode(int CorpId, string Code)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Models.Where(x => x.companyid == CorpId && x.code.Equals(Code)).FirstOrDefault();
            return res;
        }

        public ResultModel GetById(int Id)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Models.Where(x => x.id == Id).FirstOrDefault();
            return res;
        }

        public ResultModel GetByName(int CorpId, string Name)
        {
            ResultModel res = new ResultModel();
            res.Data = _db.Models.Where(x => x.companyid == CorpId && x.code.Contains(Name)).ToList();
            return res;
        }
    }
}
