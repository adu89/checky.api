using System;
namespace Checky.api.ViewBuilder
{
    public interface IViewBuilder<T,U>
    {
        T Build(U u);
    }
}
