using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using AutoMapper;
using BusinessEntities;
using DataModel;
using DataModel.UnitOfWork;

namespace BusinessServices
{
    public class SessionServices : ISessionServices
    {
        private readonly UnitOfWork _unitOfWork;
        public SessionServices(UnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public BusinessEntities.SessionEntity CreateSession(BusinessEntities.SessionEntity sessionEntity)
        {
            if (sessionEntity == null) return null;
            using (var scope = new TransactionScope())
            {
                var session = Mapper.Map<SessionEntity, Session>(sessionEntity);
                _unitOfWork.SessionRepository.Insert(session);
                _unitOfWork.Save();
                scope.Complete();
                sessionEntity.id = session.Id;
                return sessionEntity;
            }

        }


        public bool ValidateSession(string authToken)
        {
            var session = _unitOfWork.SessionRepository.Get(c => c.AuthToken == authToken && c.ExpiredOn > DateTime.Now);
            return session != null && !(DateTime.Now > session.ExpiredOn);
        }
    }
}
