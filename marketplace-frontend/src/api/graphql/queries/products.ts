import { gql } from "@apollo/client";

export const GET_NEWEST_PRODUCTS = gql`
  query GetNewestProducts {
    products(last: 10) {
      edges {
        cursor
        node {
          id
          name
          description
          price
          count
          category {
            id
            name
          }
        }
      }
      pageInfo {
        hasNextPage
        endCursor
      }
      totalCount
    }
  }
`;