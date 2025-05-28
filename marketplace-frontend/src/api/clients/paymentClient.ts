import { ApolloClient, InMemoryCache, HttpLink } from '@apollo/client';

export const paymentClient = new ApolloClient({
  cache: new InMemoryCache(),
  link: new HttpLink({
    uri: 'https://localhost:7155/graphql/',
  }),
});
