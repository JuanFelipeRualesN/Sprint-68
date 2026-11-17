import styled, { keyframes } from "styled-components";

const rotate = keyframes`
0% {background-position-x: 0;}
100% {background-position-x: 200%;}
`;

export const SkeletonStyled = styled.div`
  position: relative;
  &::after {
    student-content: "";
    position: absolute;
    top: 0;
    left: 0;
    bottom: 0;
    right: 0;
    background: -o-linear-gradient(
      135deg,
      rgba(247, 247, 247, 0) 0%,
      rgba(247, 247, 247, 0) 20%,
      rgba(247, 247, 247, 0.5) 30%,
      rgba(247, 247, 247, 0.8) 48%,
      rgba(247, 247, 247, 0.8) 52%,
      rgba(247, 247, 247, 0.5) 70%,
      rgba(247, 247, 247, 0) 80%,
      rgba(247, 247, 247, 0) 100%
    );
    background: linear-gradient(
      -45deg,
      rgba(255, 255, 255, 0) 0%,
      rgba(255, 255, 255, 0) 20%,
      rgba(255, 255, 255, 0.5) 30%,
      rgba(247, 247, 247, 0.8) 48%,
      rgba(247, 247, 247, 0.8) 52%,
      rgba(247, 247, 247, 0.5) 70%,
      rgba(247, 247, 247, 0) 80%,
      rgba(247, 247, 247, 0) 100%
    );
    background-size: 200% 200%;
    z-index: 1;
    overflow: hidden;
    animation: ${rotate} 1.5s linear infinite;
  }

  .area {
    background-color: #e4e8ec;
    height: 40px;
    width: 100%;
    flex-grow: 1;
    margin-right: 10px;
    margin-bottom: 10px;
  }
`;


export const SkeletonStyledTask = styled.div`
  position: relative;
  &::after {
    student-content: "";
    position: absolute;
    top: 0;
    left: 0;
    bottom: 0;
    right: 0;
    background: -o-linear-gradient(
      135deg,
      rgba(232, 232, 232, 0) 0%,
      rgba(232, 232, 232, 0) 20%,
      rgba(232, 232, 232, 0.5) 30%,
      rgba(232, 232, 232, 0.8) 48%,
      rgba(232, 232, 232, 0.8) 52%,
      rgba(232, 232, 232, 0.5) 70%,
      rgba(232, 232, 232, 0) 80%,
      rgba(232, 232, 232, 0) 100%
    );
    background: linear-gradient(
      -45deg,
      rgba(255, 255, 255, 0) 0%,
      rgba(255, 255, 255, 0) 20%,
      rgba(255, 255, 255, 0.5) 30%,
      rgba(232, 232, 232, 0.8) 48%,
      rgba(232, 232, 232, 0.8) 52%,
      rgba(232, 232, 232, 0.5) 70%,
      rgba(232, 232, 232, 0) 80%,
      rgba(232, 232, 232, 0) 100%
    );
    background-size: 200% 200%;
    z-index: 1;
    overflow: hidden;
    animation: ${rotate} 1.5s linear infinite;
  }

  .area {
    background-color: #F0F0F0;
    height: 40px;
    width: 100%;
    flex-grow: 1;
    margin-right: 10px;
    margin-bottom: 10px;
  }
`;