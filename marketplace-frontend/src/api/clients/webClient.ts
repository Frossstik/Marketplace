import {
  ApolloClient,
  InMemoryCache,
  HttpLink,
  ApolloLink,
  concat,
} from '@apollo/client';

const httpLink = new HttpLink({
  uri: 'https://localhost:7239/graphql/'
});

const authMiddleware = new ApolloLink((operation, forward) => {
  const token = localStorage.getItem('token'); 

  if (token) {
    operation.setContext({
      headers: {
        Authorization: `Bearer ${token}`,
      },
    });
  }

  return forward(operation);
});

export const marketplaceClient = new ApolloClient({
  cache: new InMemoryCache(),
  link: concat(authMiddleware, httpLink),
});