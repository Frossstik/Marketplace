import { useKeenSlider } from "keen-slider/react";
import "keen-slider/keen-slider.min.css";
import classNames from "classnames";
import { useState } from "react";

type Props = {
  images: string[];
};

const ImageCarousel = ({ images }: Props) => {
  const [currentSlide, setCurrentSlide] = useState(0);

  const [sliderRef, instanceRef] = useKeenSlider<HTMLDivElement>({
    initial: 0,
    loop: true,
    slideChanged: (slider) => setCurrentSlide(slider.track.details.rel),
  });

  const prev = () => instanceRef.current?.prev();
  const next = () => instanceRef.current?.next();

  return (
    <div className="relative">
      <div ref={sliderRef} className="keen-slider rounded overflow-hidden">
        {images.map((img, i) => (
          <div key={i} className="keen-slider__slide flex justify-center items-center">
            <img
              src={img}
              alt={`Изображение ${i + 1}`}
              className="h-64 w-full object-contain"
            />
          </div>
        ))}
      </div>

      {/* Стрелки */}
      <button
        onClick={prev}
        className="absolute top-1/2 -translate-y-1/2 left-2 bg-white bg-opacity-80 hover:bg-opacity-100 text-black px-2 py-1 rounded shadow"
      >
        ◀
      </button>
      <button
        onClick={next}
        className="absolute top-1/2 -translate-y-1/2 right-2 bg-white bg-opacity-80 hover:bg-opacity-100 text-black px-2 py-1 rounded shadow"
      >
        ▶
      </button>

      {/* Точки */}
      <div className="flex justify-center gap-1 mt-4">
        {images.map((_, i) => (
          <div
            key={i}
            className={classNames("w-2 h-2 rounded-full", {
              "bg-blue-600": currentSlide === i,
              "bg-gray-300": currentSlide !== i,
            })}
          />
        ))}
      </div>
    </div>
  );
};

export default ImageCarousel;
