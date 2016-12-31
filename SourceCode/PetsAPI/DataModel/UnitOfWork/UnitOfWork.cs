using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using DataModel.GenericRepository;

namespace DataModel.UnitOfWork
{
    /// <summary>
    /// Unit of Work class responsible for DB transactions
    /// </summary>
    public class UnitOfWork : IDisposable
    {
        #region Private member variables...

        private readonly PetsAPI4DevConnectionEntities _context;
        private GenericRepository<User> _userRepository;
        private GenericRepository<UserAuthInfo> _userAuthInfoRepository;
        private GenericRepository<Session> _sessionRepository;
        private GenericRepository<OTP> _otpRepository;
        private GenericRepository<Article> _articleRepository;
        private GenericRepository<Pet> _petRepository;
        private GenericRepository<Image> _imageRepository;
        #endregion

        public UnitOfWork()
        {
            _context = new PetsAPI4DevConnectionEntities();
        }

        #region Public Repository Creation properties...

        /// <summary>
        /// Get/Set Property for product repository.
        /// </summary>
        public GenericRepository<User> UserRepository
        {
            get { return _userRepository ?? (_userRepository = new GenericRepository<User>(_context)); }
        }

        /// <summary>
        /// Get/Set Property for user repository.
        /// </summary>
        public GenericRepository<UserAuthInfo> UserAuthInfoRepository
        {
            get
            {
                return _userAuthInfoRepository ??
                       (_userAuthInfoRepository = new GenericRepository<UserAuthInfo>(_context));
            }
        }

        /// <summary>
        /// Get/Set Property for token repository.
        /// </summary>
        public GenericRepository<Session> SessionRepository
        {
            get { return _sessionRepository ?? (_sessionRepository = new GenericRepository<Session>(_context)); }
        }

        /// <summary>
        /// Get/Set Property for token repository.
        /// </summary>
        public GenericRepository<OTP> OtpRepository
        {
            get { return _otpRepository ?? (_otpRepository = new GenericRepository<OTP>(_context)); }
        }

        /// <summary>
        /// Get/Set Property for token repository.
        /// </summary>
        public GenericRepository<Article> ArticleRepository
        {
            get { return _articleRepository ?? (_articleRepository = new GenericRepository<Article>(_context)); }
        }
        /// <summary>
        /// Get/Set Property for token repository.
        /// </summary>
        public GenericRepository<Pet> PetRepository
        {
            get { return _petRepository ?? (_petRepository = new GenericRepository<Pet>(_context)); }
        }
        /// <summary>
        /// Get/Set Property for token repository.
        /// </summary>
        public GenericRepository<Image> ImageRepository
        {
            get { return _imageRepository ?? (_imageRepository = new GenericRepository<Image>(_context)); }
        }
        #endregion

        #region Public member methods...
        /// <summary>
        /// Save method.
        /// </summary>
        public void Save()
        {
            try
            {
                _context.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    outputLines.AddRange(eve.ValidationErrors.Select(ve => string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage)));
                }
                System.IO.File.AppendAllLines("~/errors.txt", outputLines);

                throw;
            }

        }

        public void SaveAsync()
        {
            try
            {
                _context.SaveChangesAsync();
            }
            catch (DbEntityValidationException e)
            {

                var outputLines = new List<string>();
                foreach (var eve in e.EntityValidationErrors)
                {
                    outputLines.Add(string.Format(
                        "{0}: Entity of type \"{1}\" in state \"{2}\" has the following validation errors:", DateTime.Now,
                        eve.Entry.Entity.GetType().Name, eve.Entry.State));
                    outputLines.AddRange(eve.ValidationErrors.Select(ve => string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage)));
                }
                System.IO.File.AppendAllLines(@"D:\errors.txt", outputLines);

                throw;
            }

        }

        #endregion

        #region Implementing IDiosposable...

        #region private dispose variable declaration...
        private bool _disposed;
        #endregion

        /// <summary>
        /// Protected Virtual Dispose method
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    Debug.WriteLine("UnitOfWork is being disposed");
                    _context.Dispose();
                }
            }
            _disposed = true;
        }

        /// <summary>
        /// Dispose method
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
