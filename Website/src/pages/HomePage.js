import React from 'react';

import Hero from '../components/Hero';
import Carousel from '../components/Carousel';
import Footer from '../components/Footer'; 


function HomePage(props) {
    return (
        <div className='main-container'>
            <Hero title = {props.title} subTitle = {props.subTitle} text = {props.text}/>
        </div>
        
    );
}

export default HomePage;