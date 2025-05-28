import { gql } from "@apollo/client";

export const GET_NEWEST_PRODUCTS = gql`
  query GetNewestProducts {
    products(last: 4) {
      edges {
        node {
          id
          name
          description
          price
          count
          imagePaths
          creatorName
          creatorsCompanyName
          category {
            id
            name
          }
          createdAt
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

export const GET_PRODUCTS = gql`
  query GetProducts($first: Int = 25, $after: String) {
    products(first: $first, after: $after) {
      edges {
        cursor
        node {
          id
          name
          description
          price
          count
          imagePaths
          creatorName
          creatorsCompanyName
          category {
            id
            name
          }
          createdAt
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

// 🔹 Альтернативный (через before + last)
export const GET_PRODUCTS_CURSOR = gql`
  query GetProducts($last: Int!, $before: String) {
    products(last: $last, before: $before) {
      edges {
        cursor
        node {
          id
          name
          description
          price
          count
          imagePaths
          creatorName
          creatorsCompanyName
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

export const GET_PRODUCT = gql`
  query GetProduct($id: ID!) {
    productById(id: $id) {
      id
      name
      description
      price
      count
      imagePaths
      category {
        name
      }
      creator {
        companyName
      }
    }
  }
`;

export const SEARCH_PRODUCTS = gql`
  query SearchProducts($filter: ProductFilterInput) {
    products(where: $filter) {
      edges {
        node {
          id
          name
          price
          description
          category {
            name
          }
          imagePaths
        }
      }
    }
  }
`;

export const PRODUCTS_BY_CATEGORY = gql`
  query ProductsByCategory($categoryId: UUID!) {
    products(where: { categoryId: { eq: $categoryId } }) {
      nodes {
        id
        name
        price
        count
        description
        imagePaths
        category {
          name
        }
        creator {
          companyName
        }
      }
    }
  }
`;

// export const GET_MY_PRODUCTS = gql`
//   query GetMyProducts($creatorId: UUID!) {
//     products(where: { creatorId: { eq: $creatorId } }) {
//       nodes {
//         id
//         name
//         description
//         price
//         count
//         imagePaths
//         category {
//           name
//         }
//         creator {
//           companyName
//           firstName
//         }
//       }
//     }
//   }
// `;

export const PRODUCTS_BY_CREATOR = gql`
  query ProductsByCreator($creatorId: UUID!) {
    products(where: { creatorId: { eq: $creatorId } }) {
      nodes {
        id
        name
        price
        count
        description
        imagePaths
        category {
          name
        }
        creator {
          id
          companyName
        }
      }
    }
  }
`;


