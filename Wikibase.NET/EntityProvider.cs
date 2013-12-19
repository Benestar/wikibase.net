using System;
using System.Collections.Generic;
using System.Text;

namespace Wikibase
{
    /// <summary>
    /// Class for getting entities from various requests.
    /// </summary>
    public class EntityProvider
    {
        private WikibaseApi api;

        /// <summary>
        /// Creates a new <see cref="EntityProvider"/>.
        /// </summary>
        /// <param name="api">The api.</param>
        public EntityProvider(WikibaseApi api)
        {
            this.api = api;
        }

        /// <summary>
        /// Get the entities from the given entity ids.
        /// </summary>
        /// <param name="ids">The entity ids.</param>
        /// <returns>The entities.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ids"/> is <c>null</c>.</exception>
        public Entity[] getEntitiesFromIds(EntityId[] ids)
        {
            if ( ids == null )
                throw new ArgumentNullException("ids");

            return this.getEntitiesFromIds(ids, null);
        }

        /// <summary>
        /// Get the entities from the given entity ids with data in the languages provided.
        /// </summary>
        /// <param name="ids">The entity ids.</param>
        /// <param name="languages">The languages. <c>null</c> to get all languages.</param>
        /// <returns>The entities.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="ids"/> is <c>null</c>.</exception>
        public Entity[] getEntitiesFromIds(EntityId[] ids, string[] languages)
        {
            if ( ids == null )
                throw new ArgumentNullException("ids");

            string[] prefixedIds = new string[ids.Length];
            for ( int i = 0 ; i < ids.Length ; i++ )
            {
                prefixedIds[i] = ids[i].getPrefixedId();
            }
            return this.api.getEntitiesFromIds(prefixedIds, languages);
        }

        /// <summary>
        /// Get the entity from the given entity id.
        /// </summary>
        /// <param name="id">The entity id</param>
        /// <returns>The entity</returns>
        public Entity getEntityFromId(EntityId id)
        {
            return this.getEntityFromId(id, null);
        }

        /// <summary>
        /// Get the entity from the given entity id with data in the languages provided.
        /// </summary>
        /// <param name="id">The entity id</param>
        /// <param name="languages">The languages</param>
        /// <returns>The entity</returns>
        public Entity getEntityFromId(EntityId id, string[] languages)
        {
            Entity[] entities = this.getEntitiesFromIds(new EntityId[] { id }, languages);
            foreach ( Entity entity in entities )
            {
                return entity;
            }
            return null;
        }

        /// <summary>
        /// Get the entities from the given sites and titles.
        /// </summary>
        /// <param name="sites">The sites</param>
        /// <param name="titles">The titles</param>
        /// <returns>The entities</returns>
        public Entity[] getEntitiesFromSitelinks(string[] sites, string[] titles)
        {
            return this.getEntitiesFromSitelinks(sites, titles, null);
        }

        /// <summary>
        /// Get the entities from the given sites and titles with data in the languages provided.
        /// </summary>
        /// <param name="sites">The sites</param>
        /// <param name="titles">The titles</param>
        /// <param name="languages">The languages</param>
        /// <returns>The entities</returns>
        public Entity[] getEntitiesFromSitelinks(string[] sites, string[] titles, string[] languages)
        {
            return this.api.getEntitesFromSitelinks(sites, titles, languages);
        }

        /// <summary>
        /// Get the entity from the given site and title.
        /// </summary>
        /// <param name="site">The site</param>
        /// <param name="title">The title</param>
        /// <returns>The entity</returns>
        public Entity getEntityFromSitelink(string site, string title)
        {
            return this.getEntityFromSitelink(site, title, null);
        }

        /// <summary>
        /// Get the entity from the given site and title with data in the languages provided.
        /// </summary>
        /// <param name="site">The site</param>
        /// <param name="title">The title</param>
        /// <param name="languages">The languages</param>
        /// <returns>The entity</returns>
        public Entity getEntityFromSitelink(string site, string title, string[] languages)
        {
            Entity[] entities = this.getEntitiesFromSitelinks(new string[] { site }, new string[] { title }, languages);
            foreach ( Entity entity in entities )
            {
                return entity;
            }
            return null;
        }
    }
}