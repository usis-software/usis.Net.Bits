using System;
using System.Collections.Generic;

namespace usis.Framework
{
    public class MetadataRepository
    {
        private const string namespaceSeparator = ".";

        private Dictionary<string, NamespaceMetadata> namespaces = new Dictionary<string, NamespaceMetadata>();

        public void Import(NamespaceMetadata metadata)
        {
            foreach (var item in EnumerateNamespaces(metadata, null))
            {
                if (!namespaces.TryGetValue(item.Item1, out NamespaceMetadata ns))
                {
                    namespaces.Add(item.Item1, ns = new NamespaceMetadata(item.Item1));
                }
                ns.AddRange(item.Item2.CloneItems());
            }
        }

        #region private methods

        //  --------------------------
        //  EnumerateNamespaces method
        //  --------------------------

        private IEnumerable<Tuple<string, NamespaceMetadata>> EnumerateNamespaces(NamespaceMetadata metadata, string container)
        {
            var name = container == null ? metadata.Name : string.Join(namespaceSeparator, container, metadata.Name);
            yield return new Tuple<string, NamespaceMetadata>(name, metadata);
            foreach (var item in metadata.Namespaces)
            {
                foreach (var tuple in EnumerateNamespaces(item, name))
                {
                    yield return tuple;
                }
            }
        }

        #endregion private methods
    }
}
