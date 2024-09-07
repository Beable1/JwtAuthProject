using AutoMapper;
using JwtAuthProject.Core.Repositories;
using JwtAuthProject.Core.Services;
using JwtAuthProject.Core.UnitOfWork;
using JwtAuthProject.Service.Exceptions;
using Microsoft.EntityFrameworkCore;
using SharedLibrary.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace JwtAuthProject.Service.Services
{
    public class GenericService<TEntity, TDto> : IGenericService<TEntity, TDto> where TEntity : class where TDto : class
    {
        private readonly IGenericRepository<TEntity> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenericService(IGenericRepository<TEntity> repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<Response<TDto>> AddAsync(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            await _repository.AddAsync(entity);
            await _unitOfWork.CommitAsync();
            var newDto= _mapper.Map<TDto>(entity);
            return Response<TDto>.Success(newDto, 200);
        }
            
        public async Task<Response<IEnumerable<TDto>>> GetAllAsync()
        {
            var entities= await _repository.GetAllAsync();
            var entitiesDto=_mapper.Map<IEnumerable<TDto>>(entities);
            return Response<IEnumerable<TDto>>.Success(entitiesDto, 200);
        }

        public async Task<Response<TDto>> GetByIdAsync(int id)
        {
            var entity=await _repository.GetByIdAsync(id);
            var entityDto=_mapper.Map<TDto>(entity);

            if ( entity == null)
            {
                throw new NotFoundException($"{typeof(TEntity).Name} not found");
            }

            return Response<TDto>.Success(entityDto, 200);
        }

        public async Task<Response<NoContentDto>> Remove(int id)
        {
            var entity= await _repository.GetByIdAsync(id);
            
            _repository.Remove(entity);
            _unitOfWork.Commit();



            return Response<NoContentDto>.Success(200);
        }

        public Response<NoContentDto> Update(TDto dto)
        {
            var entity = _mapper.Map<TEntity>(dto);
            _repository.Update(entity);
            _unitOfWork.Commit();
            return Response<NoContentDto>.Success(200);
        }

        public async Task<Response<IEnumerable<TDto>>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            var list=_repository.Where(predicate);
            var listDto= _mapper.Map<IEnumerable<TDto>>(await list.ToListAsync());

            return Response<IEnumerable<TDto>>.Success(listDto, 200);

        }

        
    }
}
