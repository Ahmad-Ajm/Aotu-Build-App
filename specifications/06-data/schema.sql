قالب
-- PostgreSQL schema (مبدئي)
CREATE TABLE IF NOT EXISTS public.customers (
  id UUID PRIMARY KEY,
  name TEXT NOT NULL,
  email TEXT UNIQUE,
  created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS public.products (
  id UUID PRIMARY KEY,
  name TEXT NOT NULL,
  price NUMERIC(18,2) NOT NULL CHECK (price >= 0),
  created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE TABLE IF NOT EXISTS public.orders (
  id UUID PRIMARY KEY,
  customer_id UUID NOT NULL REFERENCES public.customers(id),
  total NUMERIC(18,2) NOT NULL DEFAULT 0,
  status TEXT NOT NULL,
  created_at TIMESTAMPTZ NOT NULL DEFAULT now()
);

CREATE INDEX IF NOT EXISTS ix_orders_customer_id ON public.orders(customer_id);
