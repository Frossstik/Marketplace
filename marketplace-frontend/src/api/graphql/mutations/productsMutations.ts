import { gql } from '@apollo/client';

export const CREATE_PRODUCT = gql`
  mutation CreateProduct($input: CreateProductInput!) {
    createProduct(input: $input) {
      product {
        id
        name
        price
        count
        category {
          name
        }
        creatorName
        creatorsCompanyName
        imagePaths
      }
    }
  }
`;

export const DELETE_PRODUCT = gql`
  mutation DeleteProduct($input: DeleteProductInput!) {
  deleteProduct(input: $input) {
    boolean
  }
}
`;

export const UPDATE_PRODUCT = gql`
  mutation UpdateProduct($input: UpdateProductInput!) {
  updateProduct(input: $input) {
    product {
      id
      name
      description
      price
      count
      imagePaths
      categoryId
    }
  }
}
`;

