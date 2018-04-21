Model with EF Code First
========================

1. Entity classes derived from usis.Framework.Entity.EntityBase class.
2. DBContext class derived from usis.Framework.Entity.DBContextBase class.
3. Add DbSet<TEntity> properties in DBContext.

4. Derive a model from usis.Framework.Entity.DBContextModel<DBContext> class.
5. Implement abstract DBContextModel class by overriding NewContext method
   (Implement appropriate constructor for DBContext).
