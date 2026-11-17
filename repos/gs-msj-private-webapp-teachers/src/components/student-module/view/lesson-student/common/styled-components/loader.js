import React from "react";
import styled, { keyframes } from "styled-components";

export const WrapperLoadingStyled = styled.div`
  z-index: 900;
  display: -webkit-box;
  display: -ms-flexbox;
  display: flex;
  -ms-flex-pack: distribute;
  justify-content: space-around;
  -webkit-box-align: center;
  -ms-flex-align: center;
  align-items: center;
  position: fixed;
  bottom: 0;
  left: 0;
  right: 0;
  height: 100%;
  background-color: rgba(0, 0, 0, 0.5);
`;

const rotate = keyframes`
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
`;

export const LoaderStyled = styled.div`
  display: inline-block;
  border: 4px solid #E88B19;
  border-radius: 50%;
  border-top: 4px solid #B5DFE9;
  width: 60px;
  height: 60px;
  animation: ${rotate} 2s linear infinite;
`;

export const Loader = () => (
  <WrapperLoadingStyled>
    <LoaderStyled />
  </WrapperLoadingStyled>
);
