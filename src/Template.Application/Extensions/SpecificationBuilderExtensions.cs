using Ardalis.Specification;

namespace Template.Application.Extensions {
    public static class SpecificationBuilderExtensions {
        public static ISpecificationBuilder<T> Paginate<T>(
            this ISpecificationBuilder<T> builder, int pageNumber, int pageSize) {

            if (pageNumber <= 0)
                pageNumber = 1;

            if (pageSize <= 0)
                pageSize = 10;
            else if (pageSize > 50)
                pageSize = 50;

            return builder.Skip((pageNumber - 1) * pageSize)
                          .Take(pageSize);
        }
    }
}