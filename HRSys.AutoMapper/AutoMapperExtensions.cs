using AutoMapper;
using HRSys.DTO;
using System;
using System.Collections.Generic;
using System.Text;

namespace HRSys.AutoMapper
{
    public static class AutoMapperExtensions
    {
        public static IMemberConfigurationExpression<TSource, TDestination, TMember> OnlyIfNotUpdatable<TSource, TDestination, TMember>(this IMemberConfigurationExpression<TSource, TDestination, TMember> expression) where TSource : IUpdatableDto
        {
            expression.PreCondition(s => !(s is IUpdatableDto) || !s.IsUpdateOperation);
            return expression;
        }
        public static IMemberConfigurationExpression<TSource, TDestination, TMember> OnlyIfNotUpdatableGuid<TSource, TDestination, TMember>(this IMemberConfigurationExpression<TSource, TDestination, TMember> expression) where TSource : IGuidUpdatableDto
        {
            expression.PreCondition(s => !(s is IGuidUpdatableDto) || !s.IsUpdateOperation);
            return expression;
        }
    }
}
