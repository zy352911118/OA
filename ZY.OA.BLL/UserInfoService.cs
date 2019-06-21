using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZY.OA.DALFactory;
using ZY.OA.IBLL;
using ZY.OA.IDAL;
using ZY.OA.Model;
using ZY.OA.Model.Enum;
using ZY.OA.Model.SearchModel;

namespace ZY.OA.BLL
{
    public partial class UserInfoService : BaseService<UserInfo>, IUserInfoService
    {
        #region T4生成
        //public override void SetCurrentDal()
        //{
        //    this.currentDal =DBSession.userInfoDal;
        //} 
        #endregion

        /// <summary>
        /// 多条件搜索
        /// </summary>
        /// <param name="userSearchParms"></param>
        /// <returns></returns>
        public IQueryable<UserInfo> GetPageEntityBySearch(UserSearchParms userSearchParms)
        {
            short normal=(short) DelFlagEnum.Normal;
            var temp = DBSession.UserInfoDal.GetEntities(u => u.DelFlag == normal);
           
            if (!string.IsNullOrEmpty(userSearchParms.schName))
            {
               temp= temp.Where<UserInfo>(u => u.UName.Contains(userSearchParms.schName));
            }
            
            if (!string.IsNullOrEmpty(userSearchParms.schRemark))
            {
                temp = temp.Where<UserInfo>(u=>u.Remark.Contains(userSearchParms.schRemark));
            }
         
            userSearchParms.total = temp.Count();
           
            return temp.OrderBy<UserInfo, int>(u => u.ID)
                       .Skip(userSearchParms.pageSize * (userSearchParms.pageIndex - 1))
                       .Take(userSearchParms.pageSize);
        }
        /// <summary>
        /// 设置用户角色
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="RoleIdList"></param>
        /// <returns></returns>
        public bool SetUserRoleInfo(int userID,List<int> RoleIdList)
        {
           UserInfo userInfo= DBSession.UserInfoDal.GetEntities(u => u.ID == userID).FirstOrDefault();
            if (userInfo != null)
            {
                userInfo.RoleInfo.Clear();//清除用户的所有角色
                foreach (int RoleId in RoleIdList)
                {
                    RoleInfo RoleInfo = DBSession.RoleInfoDal.GetEntities(r => r.ID == RoleId).FirstOrDefault();
                    userInfo.RoleInfo.Add(RoleInfo);//循环为用户添加角色
                }
                return DBSession.SaveChanges() > 0;
            }
            else
            {
                return false;
            }
        }
        /// <summary>
        /// 为特殊用户分配权限
        /// </summary>
        /// <param name="actionId">权限ID</param>
        /// <param name="userId">用户ID</param>
        /// <param name="HasPermission">是否允许</param>
        /// <returns></returns>
        public bool SetUserActionInfo(int actionId,int userId,bool HasPermission)
        {
            short Normal = (short)DelFlagEnum.Normal;
            //为特殊用户分配权限
            R_UserInfo_ActionInfo r_UserInfo_ActionInfo = DBSession.R_UserInfo_ActionInfoDal.GetEntities(r => r.ActionInfoID == actionId && r.UserInfoID == userId && r.DelFlag == Normal).FirstOrDefault();
            if (r_UserInfo_ActionInfo != null)
            {
                r_UserInfo_ActionInfo.HasPermission = HasPermission;//修改是否允许标识             
            }
            else
            {
                R_UserInfo_ActionInfo R_UserInfo_Act = new R_UserInfo_ActionInfo();
                R_UserInfo_Act.UserInfoID = userId;
                R_UserInfo_Act.ActionInfoID = actionId;
                R_UserInfo_Act.DelFlag = Normal;
                R_UserInfo_Act.HasPermission = HasPermission;
                DBSession.R_UserInfo_ActionInfoDal.Add(R_UserInfo_Act);
            }
            return DBSession.SaveChanges()>0;
        }

        //清除某用户的某条权限
        public bool ClearUserActionInfo(int userId, int actionId)
        {
           R_UserInfo_ActionInfo ruc= DBSession.R_UserInfo_ActionInfoDal.GetEntities(r=>r.UserInfoID==userId&&r.ActionInfoID==actionId).FirstOrDefault();
            if (ruc!=null)
            {
                DBSession.R_UserInfo_ActionInfoDal.Delete(ruc);
            }
            return DBSession.SaveChanges()>0;
        }
    }
}
