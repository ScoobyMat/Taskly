export type User = {
  id: string;
  firstName: string;
  lastName: string;
  userName: string;
  email: string;
  token: string;
}

export type LoginCreds = {
  email: string;
  password: string;
}

export type RegisterCreds = {
  firstName: string;
  lastName: string;
  username: string;
  email: string;
  password: string;
}
