import { ApolloClient, InMemoryCache, HttpLink } from '@apollo/client';

export const marketplaceClient = new ApolloClient({
  link: new HttpLink({ uri: 'https://localhost:7239/graphql/' }), // замените на ваш адрес
  cache: new InMemoryCache(),
});
