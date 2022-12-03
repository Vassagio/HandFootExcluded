namespace HandFootExcluded;

public interface IBuilder { }

public interface ICollectionBuilder<TBuilder, TType> where TBuilder : class, IBuilder
{
    ICollectionBuilder<TBuilder, TType> AddItem(ref IEnumerable<TType> storage, TType value);
    ICollectionBuilder<TBuilder, TType> AddItem(ref TType[] storage, TType value);
}

public abstract class BuilderBase : IBuilder { }

public delegate void PreSetAction<in TType>(TType storage, TType value);

public delegate void PreSetAction<in TType1, in TType2>(TType1 storage, TType2 value);

public delegate void PostSetAction<in TType>(TType storage);

public abstract class BuilderBase<TBuilder> : BuilderBase where TBuilder : class, IBuilder
{
    public TBuilder SetProperty<TType>(ref TType storage, TType value) => SetProperty(ref storage, value, null, null);
    public TBuilder SetProperty<TType>(ref TType storage, TType value, PreSetAction<TType>? preSetAction = null) => SetProperty(ref storage, value, preSetAction, null);
    public TBuilder SetProperty<TType>(ref TType storage, TType value, PostSetAction<TType>? postSetAction = null) => SetProperty(ref storage, value, null, postSetAction);

    public TBuilder SetProperty<TType>(ref TType storage, TType value, PreSetAction<TType>? preSetAction = null, PostSetAction<TType>? postSetAction = null)
    {
        if (EqualityComparer<TType>.Default.Equals(storage, value))
            return this as TBuilder ?? throw new ArgumentNullException(nameof(BuilderBase));

        preSetAction?.Invoke(storage, value);
        storage = value;
        postSetAction?.Invoke(storage);

        return this as TBuilder ?? throw new ArgumentNullException(nameof(BuilderBase));
    }
}