import { gql } from "@apollo/client";

export const GET_ORDERS = gql`
  query GetOrders($userId: UUID!) {
    userOrders(userId: $userId) {
      id
      totalPrice
      status
      createdAt
      items {
        productName
        unitPrice
        quantity
      }
    }
  }
`;

export const GET_ORDER_BY_ID = gql`
  query GetOrderById($orderId: UUID!, $userId: UUID!) {
    orderById(orderId: $orderId, userId: $userId) {
      id
      status
      totalPrice
      createdAt
      items {
        productId
        productName
        unitPrice
        quantity
      }
    }
  }
`;

